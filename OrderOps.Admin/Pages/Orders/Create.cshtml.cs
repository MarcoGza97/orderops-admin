using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Application.Orders.Interfaces;
using OrderOps.Application.Orders.Requests;
using OrderOps.Application.Products.Interfaces;

namespace OrderOps.Admin.Pages.Orders
{
    public sealed class CreateModel(
        IOrderService orderService,
        ICustomerService customerService,
        IProductService productService) : PageModel
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ICustomerService _customerService = customerService;
        private readonly IProductService _productService = productService;

        [BindProperty]
        public CreateOrderFormModel Order { get; set; } = new();

        public List<SelectListItem> Customers { get; private set; } = [];

        public List<SelectListItem> Products { get; private set; } = [];

        public async Task OnGetAsync()
        {
            await LoadCatalogsAsync();

            Order.Items =
            [
                new CreateOrderItemFormModel()
            ];
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCatalogsAsync();

            var validItems = Order.Items
                .Where(x => x.ProductId > 0 && x.Quantity > 0)
                .Select(x => new CreateOrderItemRequest(
                    x.ProductId,
                    x.Quantity))
                .ToList();

            var request = new CreateOrderRequest(
                Order.CustomerId,
                validItems);

            try
            {
                var orderId = await _orderService.CreateAsync(request);

                TempData["SuccessMessage"] = "Order created successfully.";

                return RedirectToPage("Details", new { id = orderId });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

                return Page();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return Page();
            }
        }

        private async Task LoadCatalogsAsync()
        {
            var customers = await _customerService.GetAllAsync();
            var products = await _productService.GetAllAsync();

            Customers = [.. customers
                .Select(customer => new SelectListItem
                {
                    Value = customer.Id.ToString(),
                    Text = $"{customer.Name} - {customer.Email}"
                })];

            Products = [.. products
                .Select(product => new SelectListItem
                {
                    Value = product.Id.ToString(),
                    Text = $"{product.Name} | Stock: {product.Stock} | Price: {product.Price:C}"
                })];
        }
    }

    public sealed class CreateOrderFormModel
    {
        public int CustomerId { get; set; }

        public List<CreateOrderItemFormModel> Items { get; set; } = [];
    }

    public sealed class CreateOrderItemFormModel
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
