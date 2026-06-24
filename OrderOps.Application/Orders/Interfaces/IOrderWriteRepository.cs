using OrderOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Interfaces
{
    public interface IOrderWriteRepository
    {
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<List<Product>> GetProductsByIdsAsync(IReadOnlyList<int> ids);
        Task<Order?> GetOrderWithItemsAsync(int id);
        Task AddOrderAsync(Order order);
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
