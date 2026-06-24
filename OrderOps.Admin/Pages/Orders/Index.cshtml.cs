using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Orders.Dtos;
using OrderOps.Application.Orders.Interfaces;

namespace OrderOps.Admin.Pages.Orders
{
    public sealed class IndexModel(IOrderService orderService) : PageModel
    {
        private readonly IOrderService _orderService = orderService;

        public IReadOnlyList<OrderSummaryDto> Orders { get; private set; } = [];

        public async Task OnGetAsync()
        {
            Orders = await _orderService.GetAllAsync();
        }
    }
}
