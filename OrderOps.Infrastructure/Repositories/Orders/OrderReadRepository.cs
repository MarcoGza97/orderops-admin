using Dapper;
using OrderOps.Application.Orders.Dtos;
using OrderOps.Application.Orders.Interfaces;
using OrderOps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Infrastructure.Repositories.Orders
{
    public sealed class OrderReadRepository(SqlConnectionFactory connectionFactory) : IOrderReadRepository
    {
        private readonly SqlConnectionFactory _connectionFactory = connectionFactory;

        public async Task<IReadOnlyList<OrderSummaryDto>> GetAllAsync()
        {
            const string sql = """
            SELECT
                o.Id,
                c.Name AS CustomerName,
                o.CreatedAt,
                CAST(o.Status AS varchar(50)) AS Status,
                SUM(oi.Quantity * oi.UnitPrice) AS Total
            FROM Orders o
            INNER JOIN Customers c ON c.Id = o.CustomerId
            INNER JOIN OrderItems oi ON oi.OrderId = o.Id
            GROUP BY o.Id, c.Name, o.CreatedAt, o.Status
            ORDER BY o.CreatedAt DESC;
            """;

            await using var connection = _connectionFactory.CreateConnection();

            var orders = await connection.QueryAsync<OrderSummaryDto>(sql);

            return [.. orders];
        }

        public async Task<OrderDetailsDto?> GetByIdAsync(int id)
        {
            const string orderSql = """
            SELECT
                o.Id,
                o.CustomerId,
                c.Name AS CustomerName,
                o.CreatedAt,
                CAST(o.Status AS varchar(50)) AS Status,
                SUM(oi.Quantity * oi.UnitPrice) AS Total
            FROM Orders o
            INNER JOIN Customers c ON c.Id = o.CustomerId
            INNER JOIN OrderItems oi ON oi.OrderId = o.Id
            WHERE o.Id = @Id
            GROUP BY o.Id, o.CustomerId, c.Name, o.CreatedAt, o.Status;
            """;

            const string itemsSql = """
            SELECT
                oi.ProductId,
                p.Name AS ProductName,
                oi.Quantity,
                oi.UnitPrice,
                oi.Quantity * oi.UnitPrice AS Total
            FROM OrderItems oi
            INNER JOIN Products p ON p.Id = oi.ProductId
            WHERE oi.OrderId = @Id
            ORDER BY p.Name;
            """;

            await using var connection = _connectionFactory.CreateConnection();

            var order = await connection.QueryFirstOrDefaultAsync<OrderDetailsDto>(
                orderSql,
                new { Id = id });

            if (order is null)
            {
                return null;
            }

            var items = await connection.QueryAsync<OrderItemDto>(itemsSql, new { Id = id });

            return order with
            {
                Items = [.. items]
            };
        }
    }
}
