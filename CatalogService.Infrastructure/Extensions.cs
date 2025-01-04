using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure
{
    public static class Extensions
    {
        public static void AddDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<CatalogDbContext>(x => x.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
