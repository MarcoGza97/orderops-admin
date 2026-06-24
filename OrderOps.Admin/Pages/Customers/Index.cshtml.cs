using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderOps.Application.Customers.Dtos;
using OrderOps.Application.Customers.Interfaces;

namespace OrderOps.Admin.Pages.Customers
{
    public sealed class IndexModel(ICustomerService customerService) : PageModel
    {
        private readonly ICustomerService _customerService = customerService;

        public IReadOnlyList<CustomerDto> Customers { get; private set; } = [];

        public async Task OnGetAsync()
        {
            Customers = await _customerService.GetAllAsync();
        }
    }
}
