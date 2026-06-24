using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Application.Products.Request;

namespace OrderOps.Admin.Pages.Products
{
    public sealed class EditModel(IProductService productService) : PageModel
    {
        private readonly IProductService _productService = productService;

        [BindProperty]
        public UpdateProductRequest Product { get; set; } = new(
            Id: 0,
            Name: string.Empty,
            Price: 0,
            Stock: 0);

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _productService.GetForEditAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            Product = product;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _productService.UpdateAsync(Product);

                TempData["SuccessMessage"] = "Product updated successfully.";

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
