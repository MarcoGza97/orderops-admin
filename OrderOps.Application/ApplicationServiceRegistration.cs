using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }
    }
}
