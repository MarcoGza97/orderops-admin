using Microsoft.EntityFrameworkCore;
using OrderOps.Application.Orders.Interfaces;
using OrderOps.Domain.Entities;
using OrderOps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Infrastructure.Repositories.Orders
{
    public sealed class OrderWriteRepository(OrderOpsDbContext context) : IOrderWriteRepository
    {
        private readonly OrderOpsDbContext _context = context;

        public Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Product>> GetProductsByIdsAsync(IReadOnlyList<int> ids)
        {
            return _context.Products
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public Task<Order?> GetOrderWithItemsAsync(int id)
        {
            return _context.Orders
                .Include(order => order.Items)
                .FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await action();

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}
