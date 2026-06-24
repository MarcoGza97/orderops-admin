using OrderOps.Application.Orders.Dtos;
using OrderOps.Application.Orders.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Interfaces
{
    public interface IOrderService
    {
        Task<IReadOnlyList<OrderSummaryDto>> GetAllAsync();
        Task<OrderDetailsDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateOrderRequest request);
        Task CancelAsync(int id);
    }
}
