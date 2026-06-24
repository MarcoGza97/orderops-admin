using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Requests
{
    public sealed record CreateOrderItemRequest(int ProductId, int Quantity);
}
