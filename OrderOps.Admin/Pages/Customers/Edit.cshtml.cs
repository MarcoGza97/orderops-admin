using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Application.Customers.Requests;

namespace OrderOps.Admin.Pages.Customers
{
    public sealed class EditModel(ICustomerService customerService) : PageModel
    {
        private readonly ICustomerService _customerService = customerService;

        [BindProperty]
        public UpdateCustomerRequest Customer { get; set; } = new(
            Id: 0,
            Name: string.Empty,
            Email: string.Empty);

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var customer = await _customerService.GetForEditAsync(id);

            if (customer is null)
                return NotFound();

            Customer = customer;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerService.UpdateAsync(Customer);

                TempData["SuccessMessage"] = "Customer updated successfully.";

                return RedirectToPage("Index");
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError($"Customer.{error.PropertyName}", error.ErrorMessage);
                }

                return Page();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return Page();
            }
        }
    }
}
