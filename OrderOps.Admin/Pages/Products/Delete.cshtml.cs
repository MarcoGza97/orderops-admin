using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Products.Dtos;
using OrderOps.Application.Products.Interfaces;

namespace OrderOps.Admin.Pages.Products
{
    public sealed class DeleteModel(IProductService productService) : PageModel
    {
        private readonly IProductService _productService = productService;

        public ProductDto? Product { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productService.GetByIdAsync(id);

            if (Product is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);

                TempData["SuccessMessage"] = "Product deleted successfully.";

                return RedirectToPage("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                Product = await _productService.GetByIdAsync(id);

                if (Product is null)
                {
                    return NotFound();
                }

                return Page();
            }
        }
    }
}
