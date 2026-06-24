using FluentValidation;
using OrderOps.Application.Orders.Dtos;
using OrderOps.Application.Orders.Interfaces;
using OrderOps.Application.Orders.Requests;
using OrderOps.Domain.Entities;
using OrderOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Services
{
    public sealed class OrderService(
        IOrderReadRepository readRepository,
        IOrderWriteRepository writeRepository,
        IValidator<CreateOrderRequest> validator) : IOrderService
    {
        private readonly IOrderReadRepository _readRepository = readRepository;
        private readonly IOrderWriteRepository _writeRepository = writeRepository;
        private readonly IValidator<CreateOrderRequest> _validator = validator;

        public Task<IReadOnlyList<OrderSummaryDto>> GetAllAsync()
        {
            return _readRepository.GetAllAsync();
        }

        public Task<OrderDetailsDto?> GetByIdAsync(int id)
        {
            return _readRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(CreateOrderRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);

            var customer = await _writeRepository.GetCustomerByIdAsync(request.CustomerId) ?? throw new InvalidOperationException("Customer not found.");

            var productIds = request.Items
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = await _writeRepository.GetProductsByIdsAsync(productIds);

            if (products.Count != productIds.Count)
                throw new InvalidOperationException("One or more products were not found.");

            var orderId = 0;

            await _writeRepository.ExecuteInTransactionAsync(async () =>
            {
                foreach (var item in request.Items)
                {
                    var product = products.First(x => x.Id == item.ProductId);

                    if (product.Stock < item.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for product {product.Name}.");

                    product.Stock -= item.Quantity;
                }

                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    CreatedAt = DateTime.UtcNow,
                    Status = OrderStatus.Completed,
                    Items = [.. request.Items.Select(item =>
                    {
                        var product = products.First(x => x.Id == item.ProductId);

                        return new OrderItem
                        {
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            UnitPrice = product.Price
                        };
                    })]
                };

                await _writeRepository.AddOrderAsync(order);

                orderId = order.Id;
            });

            return orderId;
        }

        public async Task CancelAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Order id is required.");

            await _writeRepository.ExecuteInTransactionAsync(async () =>
            {
                var order = await _writeRepository.GetOrderWithItemsAsync(id) ?? throw new InvalidOperationException("Order not found.");

                if (order.Status == OrderStatus.Cancelled)
                    throw new InvalidOperationException("Order is already cancelled.");

                var productIds = order.Items
                    .Select(item => item.ProductId)
                    .ToList();

                var products = await _writeRepository.GetProductsByIdsAsync(productIds);

                foreach (var item in order.Items)
                {
                    var product = products.First(product => product.Id == item.ProductId);

                    product.Stock += item.Quantity;
                }

                order.Status = OrderStatus.Cancelled;
            });
        }


    }
}
