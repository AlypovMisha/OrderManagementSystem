using OrderService.Application.DTOs;
namespace OrderService.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(List<OrderItemDTO> orderItemDTOs);
        Task<OrderDTO> UpdateOrderAsync(Guid id, string newStasus);
    }
}
