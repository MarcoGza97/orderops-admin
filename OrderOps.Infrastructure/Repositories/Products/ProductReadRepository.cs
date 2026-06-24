using Dapper;
using OrderOps.Application.Products.Dtos;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Domain.Entities;
using OrderOps.Infrastructure.Data;
using OrderOps.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBlueWhale.EngineQuery.Abstractions.Interfaces;
using TinyBlueWhale.EngineQuery.Core.QueryBuilding;

namespace OrderOps.Infrastructure.Repositories.Products
{
    public sealed class ProductReadRepository(SqlConnectionFactory connectionFactory,
        QueryBuilder queryBuilder) : IProductReadRepository
    {
        private readonly SqlConnectionFactory _connectionFactory = connectionFactory;
        private readonly QueryBuilder _queryEngine = queryBuilder;

        public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
        {
            //const string sql = """
            //SELECT
            //    Id,
            //    Name,
            //    Price,
            //    Stock
            //FROM Products
            //ORDER BY Name;
            //""";

            var query = _queryEngine
            .From<Product>(alias: "p")
            .Select<Product>(product => new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Stock
            })
            .OrderBy<Product>(product => product.Name)
            .Build();

            await using var connection = _connectionFactory.CreateConnection();

            var products = await connection.QueryAsync<ProductDto>(
                query.CommandText,
                query.Parameters.ToDynamicParameters());

            return [.. products];
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            //const string sql = """
            //SELECT
            //    Id,
            //    Name,
            //    Price,
            //    Stock
            //FROM Products
            //WHERE Id = @Id;
            //""";

            var query = _queryEngine
            .From<Product>(alias: "p")
            .Select<Product>(product => new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Stock
            })
            .Where<Product>(product => product.Id == id)
            .Build();

            await using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<ProductDto>(
               query.CommandText,
               query.Parameters.ToDynamicParameters());
        }
    }
}
