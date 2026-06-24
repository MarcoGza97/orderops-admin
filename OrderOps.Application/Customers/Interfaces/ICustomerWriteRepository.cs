using OrderOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Customers.Interfaces
{
    public interface ICustomerWriteRepository
    {
        Task<Customer?> GetByIdAsync(int id);

        Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);

        Task AddAsync(Customer customer);

        void Delete(Customer customer);

        Task SaveChangesAsync();
    }
}
