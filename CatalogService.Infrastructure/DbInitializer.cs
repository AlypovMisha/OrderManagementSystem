using CatalogService.Infrastructure.ConfigurationDB;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure
{
    public class DbInitializer(CatalogContext _catalogContext) : IAsyncInitializer
    {
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await _catalogContext.Database.MigrateAsync(cancellationToken);
        }
    }
}
