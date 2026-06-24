using FluentValidation;
using Moq;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Application.Products.Request;
using OrderOps.Application.Products.Services;
using OrderOps.Application.Products.Validators;
using OrderOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Tests.Products
{
    [TestFixture]
    public sealed class ProductServiceTests
    {
        private Mock<IProductReadRepository> _readRepository = null!;
        private Mock<IProductWriteRepository> _writeRepository = null!;
        private ProductService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _readRepository = new Mock<IProductReadRepository>();
            _writeRepository = new Mock<IProductWriteRepository>();

            _service = new ProductService(
                _readRepository.Object,
                _writeRepository.Object,
                new CreateProductRequestValidator(),
                new UpdateProductRequestValidator());
        }

        [Test]
        public async Task CreateAsync_WhenRequestIsValid_ShouldCreateProduct()
        {
            _writeRepository
                .Setup(x => x.ExistsByNameAsync("Keyboard", null))
                .ReturnsAsync(false);

            await _service.CreateAsync(new CreateProductRequest("Keyboard", 500, 10));

            _writeRepository.Verify(x => x.AddAsync(It.Is<Product>(p =>
                p.Name == "Keyboard" &&
                p.Price == 500 &&
                p.Stock == 10)), Times.Once);

            _writeRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void CreateAsync_WhenNameAlreadyExists_ShouldThrowInvalidOperationException()
        {
            _writeRepository
                .Setup(x => x.ExistsByNameAsync("Keyboard", null))
                .ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CreateAsync(new CreateProductRequest("Keyboard", 500, 10)));

            Assert.That(exception!.Message, Is.EqualTo("A product with the same name already exists."));
        }

        [Test]
        public void CreateAsync_WhenPriceIsInvalid_ShouldThrowValidationException()
        {
            var exception = Assert.ThrowsAsync<ValidationException>(async () =>
                await _service.CreateAsync(new CreateProductRequest("Keyboard", 0, 10)));

            Assert.That(exception, Is.Not.Null);
        }

        [Test]
        public async Task UpdateAsync_WhenProductExists_ShouldUpdateProduct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Old Keyboard",
                Price = 300,
                Stock = 5
            };

            _writeRepository
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            _writeRepository
                .Setup(x => x.ExistsByNameAsync("New Keyboard", 1))
                .ReturnsAsync(false);

            await _service.UpdateAsync(new UpdateProductRequest(1, "New Keyboard", 600, 20));

            Assert.That(product.Name, Is.EqualTo("New Keyboard"));
            Assert.That(product.Price, Is.EqualTo(600));
            Assert.That(product.Stock, Is.EqualTo(20));

            _writeRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void UpdateAsync_WhenProductDoesNotExist_ShouldThrowInvalidOperationException()
        {
            _writeRepository
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.UpdateAsync(new UpdateProductRequest(1, "Keyboard", 600, 20)));

            Assert.That(exception!.Message, Is.EqualTo("Product not found."));
        }
    }
}
