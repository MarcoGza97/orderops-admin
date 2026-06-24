using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Customers.Requests
{
    public sealed record UpdateCustomerRequest(int Id, string Name, string Email);
}
