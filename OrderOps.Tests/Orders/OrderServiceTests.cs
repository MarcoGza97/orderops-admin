using Moq;
using OrderOps.Application.Orders.Interfaces;
using OrderOps.Application.Orders.Requests;
using OrderOps.Application.Orders.Services;
using OrderOps.Application.Orders.Validators;
using OrderOps.Domain.Entities;
using OrderOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Tests.Orders
{
    [TestFixture]
    public sealed class OrderServiceTests
    {
        private Mock<IOrderReadRepository> _readRepository = null!;
        private Mock<IOrderWriteRepository> _writeRepository = null!;
        private OrderService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _readRepository = new Mock<IOrderReadRepository>();
            _writeRepository = new Mock<IOrderWriteRepository>();

            _writeRepository
                .Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async action => await action());

            _service = new OrderService(
                _readRepository.Object,
                _writeRepository.Object,
                new CreateOrderRequestValidator());
        }

        [Test]
        public async Task CreateAsync_WhenRequestIsValid_ShouldCreateOrderAndDiscountStock()
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Customer",
                Email = "customer@mail.com"
            };

            var product = new Product
            {
                Id = 10,
                Name = "Mouse",
                Price = 200,
                Stock = 5
            };

            _writeRepository
                .Setup(x => x.GetCustomerByIdAsync(1))
                .ReturnsAsync(customer);

            _writeRepository
                .Setup(x => x.GetProductsByIdsAsync(It.IsAny<IReadOnlyList<int>>()))
                .ReturnsAsync([product]);

            await _service.CreateAsync(new CreateOrderRequest(
                1,
                [new CreateOrderItemRequest(10, 2)]));

            Assert.That(product.Stock, Is.EqualTo(3));

            _writeRepository.Verify(x => x.AddOrderAsync(It.Is<Order>(order =>
                order.CustomerId == 1 &&
                order.Status == OrderStatus.Completed &&
                order.Items.Count == 1)), Times.Once);
        }

        [Test]
        public void CreateAsync_WhenCustomerDoesNotExist_ShouldThrowInvalidOperationException()
        {
            _writeRepository
                .Setup(x => x.GetCustomerByIdAsync(1))
                .ReturnsAsync((Customer?)null);

            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CreateAsync(new CreateOrderRequest(
                    1,
                    [new CreateOrderItemRequest(10, 1)])));

            Assert.That(exception!.Message, Is.EqualTo("Customer not found."));
        }

        [Test]
        public void CreateAsync_WhenStockIsInsufficient_ShouldThrowInvalidOperationException()
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Customer",
                Email = "customer@mail.com"
            };

            var product = new Product
            {
                Id = 10,
                Name = "Mouse",
                Price = 200,
                Stock = 1
            };

            _writeRepository
                .Setup(x => x.GetCustomerByIdAsync(1))
                .ReturnsAsync(customer);

            _writeRepository
                .Setup(x => x.GetProductsByIdsAsync(It.IsAny<IReadOnlyList<int>>()))
                .ReturnsAsync([product]);

            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CreateAsync(new CreateOrderRequest(
                    1,
                    [new CreateOrderItemRequest(10, 2)])));

            Assert.That(exception!.Message, Is.EqualTo("Insufficient stock for product Mouse."));
        }

        [Test]
        public async Task CancelAsync_WhenOrderExists_ShouldCancelOrderAndRestoreStock()
        {
            var order = new Order
            {
                Id = 1,
                Status = OrderStatus.Completed,
                Items =
                [
                    new OrderItem
                {
                    ProductId = 10,
                    Quantity = 2,
                    UnitPrice = 200
                }
                ]
            };

            var product = new Product
            {
                Id = 10,
                Name = "Mouse",
                Price = 200,
                Stock = 3
            };

            _writeRepository
                .Setup(x => x.GetOrderWithItemsAsync(1))
                .ReturnsAsync(order);

            _writeRepository
                .Setup(x => x.GetProductsByIdsAsync(It.IsAny<IReadOnlyList<int>>()))
                .ReturnsAsync([product]);

            await _service.CancelAsync(1);

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Cancelled));
            Assert.That(product.Stock, Is.EqualTo(5));
        }
    }
}
