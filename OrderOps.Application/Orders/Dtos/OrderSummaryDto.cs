using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Dtos
{
    public sealed record OrderSummaryDto(int Id,
        string CustomerName,
        DateTime CreatedAt,
        string Status,
        decimal Total);
}
