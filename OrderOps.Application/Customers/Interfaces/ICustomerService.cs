using OrderOps.Application.Customers.Dtos;
using OrderOps.Application.Customers.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Customers.Interfaces
{
    public interface ICustomerService
    {
        Task<IReadOnlyList<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<UpdateCustomerRequest?> GetForEditAsync(int id);
        Task CreateAsync(CreateCustomerRequest request);
        Task UpdateAsync(UpdateCustomerRequest request);
        Task DeleteAsync(int id);
    }
}
