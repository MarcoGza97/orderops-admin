using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Requests
{
    public sealed record CreateOrderRequest(int CustomerId, IReadOnlyList<CreateOrderItemRequest> Items);
}
