using OrderService.Core.Entities;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task SaveChangesAsync();
        Task UpdateOrderAsync(Order updateProduct);
    }
}
