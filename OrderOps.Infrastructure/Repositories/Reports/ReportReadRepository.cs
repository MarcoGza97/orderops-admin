using Dapper;
using OrderOps.Application.Reports.Dtos;
using OrderOps.Application.Reports.Interfaces;
using OrderOps.Domain.Entities;
using OrderOps.Infrastructure.Data;
using OrderOps.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBlueWhale.EngineQuery.Core.QueryBuilding;

namespace OrderOps.Infrastructure.Repositories.Reports
{
    public sealed class ReportReadRepository(
    SqlConnectionFactory connectionFactory,
    QueryBuilder queryBuilder) : IReportReadRepository
    {
        private readonly SqlConnectionFactory _connectionFactory = connectionFactory;
        private readonly QueryBuilder _queryBuilder = queryBuilder;

        public async Task<IReadOnlyList<TopProductReportDto>> GetTopProductsAsync()
        {
            const string sql = """
            SELECT TOP 10
                p.Id AS ProductId,
                p.Name AS ProductName,
                SUM(oi.Quantity) AS QuantitySold,
                SUM(oi.Quantity * oi.UnitPrice) AS Revenue
            FROM OrderItems oi
            INNER JOIN Products p ON p.Id = oi.ProductId
            INNER JOIN Orders o ON o.Id = oi.OrderId
            WHERE o.Status <> 3
            GROUP BY p.Id, p.Name
            ORDER BY QuantitySold DESC;
            """;

            await using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryAsync<TopProductReportDto>(sql);

            return [.. result];
        }

        public async Task<IReadOnlyList<SalesByCustomerReportDto>> GetSalesByCustomerAsync()
        {
            const string sql = """
            SELECT
                c.Id AS CustomerId,
                c.Name AS CustomerName,
                COUNT(DISTINCT o.Id) AS OrdersCount,
                SUM(oi.Quantity * oi.UnitPrice) AS Revenue
            FROM Orders o
            INNER JOIN Customers c ON c.Id = o.CustomerId
            INNER JOIN OrderItems oi ON oi.OrderId = o.Id
            WHERE o.Status <> 3
            GROUP BY c.Id, c.Name
            ORDER BY Revenue DESC;
            """;

            await using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryAsync<SalesByCustomerReportDto>(sql);

            return [.. result];
        }

        public async Task<IReadOnlyList<LowStockProductDto>> GetLowStockProductsAsync(int threshold)
        {
            var query = _queryBuilder
                .From<Product>(alias: "p")
                .Select<Product>(product => new
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    product.Stock
                })
                .Where<Product>(product => product.Stock <= threshold)
                .OrderBy<Product>(product => product.Stock)
                .Build();

            await using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryAsync<LowStockProductDto>(
                query.CommandText,
                query.Parameters.ToDynamicParameters());

            return [.. result];
        }
    }
}
