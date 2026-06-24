using OrderOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Products.Interfaces
{
    public interface IProductWriteRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
        Task AddAsync(Product product);
        void Delete(Product product);
        Task SaveChangesAsync();
    }
}
