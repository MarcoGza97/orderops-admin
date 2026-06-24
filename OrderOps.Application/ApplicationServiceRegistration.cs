using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Application.Customers.Requests;
using OrderOps.Application.Customers.Services;
using OrderOps.Application.Customers.Validators;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Application.Products.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}
