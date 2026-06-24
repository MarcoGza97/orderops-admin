using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Dtos
{
    public sealed record OrderItemDto(int ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal Total);
}
