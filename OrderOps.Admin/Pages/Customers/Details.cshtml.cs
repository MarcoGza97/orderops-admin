using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Customers.Dtos;
using OrderOps.Application.Customers.Interfaces;

namespace OrderOps.Admin.Pages.Customers
{
    public sealed class DetailsModel(ICustomerService customerService) : PageModel
    {
        private readonly ICustomerService _customerService = customerService;

        public CustomerDto? Customer { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _customerService.GetByIdAsync(id);

            if (Customer is null)
                return NotFound();

            return Page();
        }
    }
}
