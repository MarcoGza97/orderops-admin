using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Application.Customers.Requests;

namespace OrderOps.Admin.Pages.Customers
{
    public sealed class CreateModel(ICustomerService customerService) : PageModel
    {
        private readonly ICustomerService _customerService = customerService;

        [BindProperty]
        public CreateCustomerRequest Customer { get; set; } = new(string.Empty, string.Empty);

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerService.CreateAsync(Customer);

                TempData["SuccessMessage"] = "Customer created successfully.";

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
