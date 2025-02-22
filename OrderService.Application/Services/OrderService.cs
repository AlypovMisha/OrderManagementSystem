using FluentValidation;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Core.Entities;

namespace OrderService.Application.Services
{
    public class OrderService(ICatalogServiceClient _catalogServiceClient, IOrderRepository _orderRepository, ILogger<OrderService> _logger, IValidator<OrderItemDTO> _productValidator) : IOrderService
    {
        public async Task<OrderDTO> CreateOrderAsync(List<OrderItemDTO> items)
        {
            if (items == null || items.Count == 0)
            {
                _logger.LogWarning("The order must contain at least 1 product.");
                throw new OrderIsEmptyException("The order must contain at least 1 product.");
            }

            await ReserveProducts(_catalogServiceClient, _logger, items, _productValidator);
            Order order = await FillOrderProducts(_catalogServiceClient, items);

            await _orderRepository.CreateOrderAsync(order);
            var orderDto = new OrderDTO(order);
            orderDto.Items = new List<OrderItemDTO>(items);
            return orderDto;
        }

        public async Task<OrderDTO> UpdateOrderAsync(Guid id, string newStatus)
        {
            var updateOrder = await _orderRepository.GetOrderByIdAsync(id);

            if (updateOrder == null)
            {
                _logger.LogWarning($"Order with ID {id} is not found");
                throw new OrderNotFoundException($"Order with ID {id} not found.");
            }

            if (string.IsNullOrWhiteSpace(newStatus))
            {
                _logger.LogWarning($"Invalid status for order {id}: status is empty.");
                throw new ArgumentException("Status cannot be empty.");
            }

            if (!Enum.TryParse<OrderStatus>(newStatus, true, out var status))
            {
                _logger.LogWarning($"Invalid status for order {id}: {newStatus}");
                throw new ArgumentException($"Invalid order status: {newStatus}");
            }

            if (!IsValidStatusTransition(updateOrder.Status, status))
            {
                _logger.LogWarning($"Invalid status transition for order {id}: {updateOrder.Status} -> {status}");
                throw new InvalidOperationException($"Cannot change status from {updateOrder.Status} to {status}");
            }

            updateOrder.Status = status;
            await _orderRepository.UpdateOrderAsync(updateOrder);

            return new OrderDTO(updateOrder);
        }


        private static async Task ReserveProducts(ICatalogServiceClient _catalogServiceClient, ILogger<OrderService> _logger, List<OrderItemDTO> items, IValidator<OrderItemDTO> _productValidator)
        {
            foreach (var item in items)
            {
                var validationResult = await _productValidator.ValidateAsync(item);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning($"Validation Exeption!!!");
                    throw new ValidationException(validationResult.Errors);
                }

                if (!await _catalogServiceClient.ReserveProductAsync(item.ProductId, item.Quantity))
                {
                    _logger.LogWarning($"Error in CreateOrderAsync.");
                    throw new LackProductInStockException($"There is no such quantity of product with id - {item.ProductId} in stock. The order is not possible.");
                }
            }
        }

        private static async Task<Order> FillOrderProducts(ICatalogServiceClient _catalogServiceClient, List<OrderItemDTO> items)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
            };
            foreach (var item in items)
            {
                decimal price = await _catalogServiceClient.GetProductPriceAsync(item.ProductId);

                var orderItem = new OrderItem()
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    Order = order,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = price,
                };
                order.Items.Add(orderItem);
                order.TotalCost += orderItem.Quantity * orderItem.Price;
            }

            return order;
        }

        private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            var allowedTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
            {
                 { OrderStatus.New, new List<OrderStatus> { OrderStatus.InAssembly, OrderStatus.Cancelled } },
                 { OrderStatus.InAssembly, new List<OrderStatus> { OrderStatus.Assembled, OrderStatus.Cancelled } },
                 { OrderStatus.Assembled, new List<OrderStatus> { OrderStatus.InDelivery, OrderStatus.Cancelled } },
                 { OrderStatus.InDelivery, new List<OrderStatus> { OrderStatus.Delivered, OrderStatus.Cancelled } },
                 { OrderStatus.Delivered, new List<OrderStatus>() },
                 { OrderStatus.Cancelled, new List<OrderStatus>() }
            };

            return allowedTransitions.TryGetValue(currentStatus, out var validNextStatuses) &&
                   validNextStatuses.Contains(newStatus);
        }
    }
}
