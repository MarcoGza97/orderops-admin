using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Reports.Dtos
{
    public sealed record SalesByCustomerReportDto(int CustomerId,
        string CustomerName,
        int OrdersCount,
        decimal Revenue);
}
