using OrderService.Application.DTOs;
namespace OrderService.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto);
        Task<OrderDTO> UpdateOrderAsync(Guid id, OrderDTO orderDto);
    }
}
