using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.ConfigurationDB;

namespace OrderService.Infrastructure
{
    public class DbInitializer(OrderContext _orderContext) : IAsyncInitializer
    {
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await _orderContext.Database.MigrateAsync(cancellationToken);
        }
    }
}
