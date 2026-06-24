using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Orders.Dtos;
using OrderOps.Application.Orders.Interfaces;

namespace OrderOps.Admin.Pages.Orders
{
    public sealed class CancelModel(IOrderService orderService) : PageModel
    {
        private readonly IOrderService _orderService = orderService;

        public OrderDetailsDto? Order { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _orderService.GetByIdAsync(id);

            if (Order is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                await _orderService.CancelAsync(id);

                TempData["SuccessMessage"] = "Order cancelled successfully.";

                return RedirectToPage("Details", new { id });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                Order = await _orderService.GetByIdAsync(id);

                if (Order is null)
                    return NotFound();

                return Page();
            }
        }
    }
}
