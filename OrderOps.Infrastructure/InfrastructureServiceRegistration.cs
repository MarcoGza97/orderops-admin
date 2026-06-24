using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Application.Orders.Interfaces;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Infrastructure.Data;
using OrderOps.Infrastructure.Repositories.Customers;
using OrderOps.Infrastructure.Repositories.Orders;
using OrderOps.Infrastructure.Repositories.Products;
using TinyBlueWhale.EngineQuery.Core.QueryBuilding;
using TinyBlueWhale.EngineQuery.Metadata.EntityFramework.Resolvers;
using TinyBlueWhale.EngineQuery.SqlServer.Capabilities;
using TinyBlueWhale.EngineQuery.SqlServer.Compilation;
using TinyBlueWhale.EngineQuery.SqlServer.Dialects;


namespace OrderOps.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderOpsDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<QueryBuilder>(serviceProvider =>
            {
                var dbContext = serviceProvider.GetRequiredService<OrderOpsDbContext>();

                var metadataResolver = new EntityFrameworkMetadataResolver(dbContext.Model);

                return new QueryBuilder(
                    new SqlServerQueryCompiler(
                        new SqlServerDatabaseDialect(),
                        new SqlServerProviderCapabilities()),
                    metadataResolver);
            });

            services.AddScoped<SqlConnectionFactory>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            return services;
        }
    }
}
