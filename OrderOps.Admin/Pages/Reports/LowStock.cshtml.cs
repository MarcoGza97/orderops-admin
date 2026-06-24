using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Reports.Dtos;
using OrderOps.Application.Reports.Interfaces;

namespace OrderOps.Admin.Pages.Reports
{
    public sealed class LowStockModel(IReportService reportService) : PageModel
    {
        private readonly IReportService _reportService = reportService;

        [BindProperty(SupportsGet = true)]
        public int Threshold { get; set; } = 5;

        public IReadOnlyList<LowStockProductDto> Products { get; private set; } = [];

        public async Task OnGetAsync()
        {
            if (Threshold <= 0)
            {
                Threshold = 5;
            }

            Products = await _reportService.GetLowStockProductsAsync(Threshold);
        }
    }
}
