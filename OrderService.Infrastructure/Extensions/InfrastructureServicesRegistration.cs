using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.ConfigurationDB;
using OrderService.Infrastructure.Messaging.Producers;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure.Extensions
{
    public static class InfrastructureServicesRegistration
    {
        public static void AddProducer<TMessage>(this IServiceCollection serviceCollection, IConfigurationSection configurationSection)
        {
            serviceCollection.Configure<KafkaSettings>(configurationSection);
            serviceCollection.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();
        }

        public static void AddDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<OrderContext>(x => x.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}
