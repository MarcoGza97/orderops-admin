using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Customers.Dtos
{
    public sealed record CustomerDto(int Id,
        string Name,
        string Email);
}
