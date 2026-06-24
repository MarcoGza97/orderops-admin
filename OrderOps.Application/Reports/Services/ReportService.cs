using OrderOps.Application.Reports.Dtos;
using OrderOps.Application.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Reports.Services
{
    public sealed class ReportService(IReportReadRepository reportReadRepository) : IReportService
    {
        private readonly IReportReadRepository _reportReadRepository = reportReadRepository;

        public Task<IReadOnlyList<TopProductReportDto>> GetTopProductsAsync()
        {
            return _reportReadRepository.GetTopProductsAsync();
        }
        public Task<IReadOnlyList<SalesByCustomerReportDto>> GetSalesByCustomerAsync()
        {
            return _reportReadRepository.GetSalesByCustomerAsync();
        }
        public Task<IReadOnlyList<LowStockProductDto>> GetLowStockProductsAsync(int threshold = 5)
        {
            return _reportReadRepository.GetLowStockProductsAsync(threshold);
        }
    }
}
