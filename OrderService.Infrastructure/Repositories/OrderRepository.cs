using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Core.Entities;
using OrderService.Infrastructure.ConfigurationDB;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository(OrderContext orderContext) : IOrderRepository
    {
        public async Task CreateOrderAsync(Order order)
        {
            await orderContext.Orders.AddAsync(order);
            await orderContext.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order updateOrder)
        {
            updateOrder.UpdatedDateUtc = DateTime.UtcNow;
            orderContext.Orders.Update(updateOrder);
            await orderContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await orderContext.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await orderContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
