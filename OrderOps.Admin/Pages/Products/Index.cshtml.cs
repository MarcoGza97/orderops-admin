using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Products.Dtos;
using OrderOps.Application.Products.Interfaces;

namespace OrderOps.Admin.Pages.Products
{
    public sealed class IndexModel(IProductService productService) : PageModel
    {
        private readonly IProductService _productService = productService;

        public IReadOnlyList<ProductDto> Products { get; private set; } = [];

        public async Task OnGetAsync()
        {
            Products = await _productService.GetAllAsync();
        }
    }
}
