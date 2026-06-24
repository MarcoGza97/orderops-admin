using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Reports.Dtos
{
    public sealed record TopProductReportDto(int ProductId,
        string ProductName,
        int QuantitySold,
        decimal Revenue);
}
