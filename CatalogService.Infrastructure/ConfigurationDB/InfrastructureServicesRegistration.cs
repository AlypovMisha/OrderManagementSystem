using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.ConfigurationDB;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure.ConfigurationDB
{
    public static class InfrastructureServicesRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<CatalogContext>(x => x.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            serviceCollection.AddScoped<ICatalogRepository, CatalogRepository>();
        }
    }
}
