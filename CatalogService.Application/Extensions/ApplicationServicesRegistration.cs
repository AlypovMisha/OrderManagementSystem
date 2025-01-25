using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Application.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Application.Extensions
{
    public static class ApplicationServicesRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddValidatorsFromAssemblyContaining<ProductValidator>();
        }
    }
}
