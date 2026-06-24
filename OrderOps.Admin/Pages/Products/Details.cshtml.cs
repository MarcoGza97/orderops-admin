using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Products.Dtos;
using OrderOps.Application.Products.Interfaces;

namespace OrderOps.Admin.Pages.Products
{
    public sealed class DetailsModel(IProductService productService) : PageModel
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
    }
}
