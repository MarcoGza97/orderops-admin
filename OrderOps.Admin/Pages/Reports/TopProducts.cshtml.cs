using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Reports.Dtos;
using OrderOps.Application.Reports.Interfaces;

namespace OrderOps.Admin.Pages.Reports
{
    public sealed class TopProductsModel(IReportService reportService) : PageModel
    {
        private readonly IReportService _reportService = reportService;

        public IReadOnlyList<TopProductReportDto> Products { get; private set; } = [];

        public async Task OnGetAsync()
        {
            Products = await _reportService.GetTopProductsAsync();
        }
    }
}
