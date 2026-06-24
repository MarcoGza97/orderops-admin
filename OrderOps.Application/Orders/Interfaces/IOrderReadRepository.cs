using OrderOps.Application.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Interfaces
{
    public interface IOrderReadRepository
    {
        Task<IReadOnlyList<OrderSummaryDto>> GetAllAsync();
        Task<OrderDetailsDto?> GetByIdAsync(int id);
    }
}
