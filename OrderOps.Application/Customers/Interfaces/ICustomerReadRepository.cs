using OrderOps.Application.Customers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Customers.Interfaces
{
    public interface ICustomerReadRepository
    {
        Task<IReadOnlyList<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int id);
    }
}
