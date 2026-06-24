using Microsoft.EntityFrameworkCore;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Domain.Entities;
using OrderOps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Infrastructure.Repositories.Products
{
    public sealed class ProductWriteRepository(OrderOpsDbContext context) : IProductWriteRepository
    {
        private readonly OrderOpsDbContext _context = context;

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            var normalizedName = name.Trim().ToLower();

            return await _context.Products
                .AnyAsync(product =>
                    product.Name.ToLower() == normalizedName &&
                    (!excludeId.HasValue || product.Id != excludeId.Value));
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
