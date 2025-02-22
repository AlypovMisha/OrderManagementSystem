using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService _orderService, IKafkaProducer<OrderDTO> _kafkaProducer) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderItemDTO> listProducts)
        {
            var createdOrder = await _orderService.CreateOrderAsync(listProducts);            

            await _kafkaProducer.ProduceAsync(createdOrder, CancellationToken.None);

            return Ok(createdOrder);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string newStatus)
        {
            var updatedOrder = await _orderService.UpdateOrderAsync(id, newStatus);
            return Ok(updatedOrder);
        }
    }
}
