using OrderOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];
    }
}
