using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure.Data
{
    public static class Extensions
    {
        public static void AddDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<CatalogContext>(x => x.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
