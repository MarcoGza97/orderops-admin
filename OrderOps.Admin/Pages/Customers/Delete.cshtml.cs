using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Customers.Dtos;
using OrderOps.Application.Customers.Interfaces;

namespace OrderOps.Admin.Pages.Customers
{
    public sealed class DeleteModel(ICustomerService customerService) : PageModel
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                await _customerService.DeleteAsync(id);

                TempData["SuccessMessage"] = "Customer deleted successfully.";

                return RedirectToPage("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                Customer = await _customerService.GetByIdAsync(id);

                if (Customer is null)
                    return NotFound();

                return Page();
            }
        }
    }
}
