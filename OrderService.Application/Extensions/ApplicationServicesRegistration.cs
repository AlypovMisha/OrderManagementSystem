using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces;
using OrderService.Application.Validation;

namespace OrderService.Application.Extensions
{
    public static class ApplicationServicesRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, Services.OrderService>();
            services.AddValidatorsFromAssemblyContaining<OrderItemValidator>();
        }
    }
}
