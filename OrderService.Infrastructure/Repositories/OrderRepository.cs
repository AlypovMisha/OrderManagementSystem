using OrderService.Application.Interfaces;
using OrderService.Core.Entities;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public Task CreateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderAsync(Order updateProduct)
        {
            throw new NotImplementedException();
        }
    }
}
