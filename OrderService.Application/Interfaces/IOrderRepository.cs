using OrderService.Core.Entities;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task SaveChangesAsync();
        Task UpdateOrderAsync(Order updateProduct);
    }
}
