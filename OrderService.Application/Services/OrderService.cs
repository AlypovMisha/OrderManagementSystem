using FluentValidation;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Services
{
    public class OrderService(IOrderRepository _orderRepository, ILogger<OrderService> _logger, IValidator<OrderDTO> _orderValidator) : IOrderService
    {
        public Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO> UpdateOrderAsync(Guid id, OrderDTO orderDto)
        {
            throw new NotImplementedException();
        }
    }
}
