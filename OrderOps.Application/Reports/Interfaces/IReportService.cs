using OrderOps.Application.Reports.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Reports.Interfaces
{
    public interface IReportService
    {
        Task<IReadOnlyList<TopProductReportDto>> GetTopProductsAsync();
        Task<IReadOnlyList<SalesByCustomerReportDto>> GetSalesByCustomerAsync();
        Task<IReadOnlyList<LowStockProductDto>> GetLowStockProductsAsync(int threshold = 5);
    }
}
