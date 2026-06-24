using Dapper;
using OrderOps.Application.Products.Dtos;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Infrastructure.Repositories.Products
{
    public sealed class ProductReadRepository(SqlConnectionFactory connectionFactory) : IProductReadRepository
    {
        private readonly SqlConnectionFactory _connectionFactory = connectionFactory;

        public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
        {
            const string sql = """
            SELECT
                Id,
                Name,
                Price,
                Stock
            FROM Products
            ORDER BY Name;
            """;

            await using var connection = _connectionFactory.CreateConnection();

            var products = await connection.QueryAsync<ProductDto>(sql);

            return [.. products];
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            const string sql = """
            SELECT
                Id,
                Name,
                Price,
                Stock
            FROM Products
            WHERE Id = @Id;
            """;

            await using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<ProductDto>(
                sql,
                new { Id = id });
        }
    }
}
