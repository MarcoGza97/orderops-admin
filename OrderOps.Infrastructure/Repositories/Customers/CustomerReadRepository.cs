using Dapper;
using OrderOps.Application.Customers.Dtos;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Infrastructure.Repositories.Customers
{
    public sealed class CustomerReadRepository : ICustomerReadRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public CustomerReadRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<CustomerDto>> GetAllAsync()
        {
            const string sql = """
            SELECT Id, Name, Email
            FROM Customers
            ORDER BY Name;
            """;

            await using var connection = _connectionFactory.CreateConnection();

            var customers = await connection.QueryAsync<CustomerDto>(sql);

            return [.. customers];
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            const string sql = """
            SELECT Id, Name, Email
            FROM Customers
            WHERE Id = @Id;
            """;

            await using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<CustomerDto>(sql, new { Id = id });
        }
    }
}
