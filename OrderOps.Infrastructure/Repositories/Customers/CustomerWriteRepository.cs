using Microsoft.EntityFrameworkCore;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Domain.Entities;
using OrderOps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Infrastructure.Repositories.Customers
{
    public sealed class CustomerWriteRepository : ICustomerWriteRepository
    {
        private readonly OrderOpsDbContext _context;

        public CustomerWriteRepository(OrderOpsDbContext context)
        {
            _context = context;
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            return _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
        {
            var normalizedEmail = email.Trim();

            return await _context.Customers.AnyAsync(x =>
                x.Email == normalizedEmail &&
                (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public void Delete(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
