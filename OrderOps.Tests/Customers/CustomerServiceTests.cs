using FluentValidation;
using Moq;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Application.Customers.Requests;
using OrderOps.Application.Customers.Services;
using OrderOps.Application.Customers.Validators;
using OrderOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Tests.Customers
{
    [TestFixture]
    public sealed class CustomerServiceTests
    {
        private Mock<ICustomerReadRepository> _readRepository = null!;
        private Mock<ICustomerWriteRepository> _writeRepository = null!;
        private CustomerService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _readRepository = new Mock<ICustomerReadRepository>();
            _writeRepository = new Mock<ICustomerWriteRepository>();

            _service = new CustomerService(
                _readRepository.Object,
                _writeRepository.Object,
                new CreateCustomerRequestValidator(),
                new UpdateCustomerRequestValidator());
        }

        [Test]
        public async Task CreateAsync_WhenRequestIsValid_ShouldCreateCustomer()
        {
            _writeRepository
                .Setup(x => x.ExistsByEmailAsync("test@mail.com", null))
                .ReturnsAsync(false);

            await _service.CreateAsync(new CreateCustomerRequest("Test Customer", "test@mail.com"));

            _writeRepository.Verify(x => x.AddAsync(It.Is<Customer>(c =>
                c.Name == "Test Customer" &&
                c.Email == "test@mail.com")), Times.Once);

            _writeRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void CreateAsync_WhenEmailAlreadyExists_ShouldThrowInvalidOperationException()
        {
            _writeRepository
                .Setup(x => x.ExistsByEmailAsync("test@mail.com", null))
                .ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CreateAsync(new CreateCustomerRequest("Test Customer", "test@mail.com")));

            Assert.That(exception!.Message, Is.EqualTo("A customer with the same email already exists."));
        }

        [Test]
        public void CreateAsync_WhenEmailIsInvalid_ShouldThrowValidationException()
        {
            var exception = Assert.ThrowsAsync<ValidationException>(async () =>
                await _service.CreateAsync(new CreateCustomerRequest("Test Customer", "invalid-email")));

            Assert.That(exception, Is.Not.Null);
        }

        [Test]
        public async Task UpdateAsync_WhenCustomerExists_ShouldUpdateCustomer()
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Old Name",
                Email = "old@mail.com"
            };

            _writeRepository
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(customer);

            _writeRepository
                .Setup(x => x.ExistsByEmailAsync("new@mail.com", 1))
                .ReturnsAsync(false);

            await _service.UpdateAsync(new UpdateCustomerRequest(1, "New Name", "new@mail.com"));

            Assert.That(customer.Name, Is.EqualTo("New Name"));
            Assert.That(customer.Email, Is.EqualTo("new@mail.com"));

            _writeRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
