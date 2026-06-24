using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Products.Dtos
{
    public sealed record ProductDto(int Id,
        string Name,
        decimal Price,
        int Stock);
}
