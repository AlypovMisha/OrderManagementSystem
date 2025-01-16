using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure.Data
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
