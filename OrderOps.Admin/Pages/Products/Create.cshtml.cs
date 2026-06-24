using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Application.Products.Request;

namespace OrderOps.Admin.Pages.Products
{
    public sealed class CreateModel(IProductService productService) : PageModel
    {
        private readonly IProductService _productService = productService;

        [BindProperty]
        public CreateProductRequest Product { get; set; } = new(
            Name: string.Empty,
            Price: 0,
            Stock: 0);

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _productService.CreateAsync(Product);

                TempData["SuccessMessage"] = "Product created successfully.";

                return RedirectToPage("Index");
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError($"Product.{error.PropertyName}", error.ErrorMessage);
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
