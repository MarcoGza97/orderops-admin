using Dapper;
using OrderOps.Application.Customers.Dtos;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Domain.Entities;
using OrderOps.Infrastructure.Data;
using OrderOps.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBlueWhale.EngineQuery.Core.QueryBuilding;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderOps.Infrastructure.Repositories.Customers
{
    public sealed class CustomerReadRepository(SqlConnectionFactory connectionFactory,
         QueryBuilder queryBuilder) : ICustomerReadRepository
    {
        private readonly SqlConnectionFactory _connectionFactory = connectionFactory;

        private readonly QueryBuilder _queryEngine = queryBuilder;

        public async Task<IReadOnlyList<CustomerDto>> GetAllAsync()
        {
            //const string sql = """
            //SELECT Id, Name, Email
            //FROM Customers
            //ORDER BY Name;
            //""";
            
            var query = _queryEngine
            .From<Customer>(alias: "c")
            .Select<Customer>(customer => new
            {
                customer.Id,
                customer.Name,
                customer.Email
            })
            .OrderBy<Customer>(customer => customer.Name)
            .Build();

            await using var connection = _connectionFactory.CreateConnection();

            var customers = await connection.QueryAsync<CustomerDto>(query.CommandText, query.Parameters.ToDynamicParameters());

            return [.. customers];
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            //const string sql = """
            //SELECT Id, Name, Email
            //FROM Customers
            //WHERE Id = @Id;
            //""";

            var query = _queryEngine
           .From<Customer>(alias: "c")
           .Select<Customer>(customer => new
           {
               customer.Id,
               customer.Name,
               customer.Email
           })
           .Where<Customer>(customer => customer.Id == id)
           .Build();

            await using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<CustomerDto>(query.CommandText, query.Parameters.ToDynamicParameters());
        }
    }
}
