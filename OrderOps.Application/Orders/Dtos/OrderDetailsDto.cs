using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Dtos
{
    public sealed record OrderDetailsDto
    {
        public int Id { get; init; }
        public int CustomerId { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public string Status { get; init; } = string.Empty;
        public decimal Total { get; init; }
        public IReadOnlyList<OrderItemDto> Items { get; init; } = [];
    }
}
