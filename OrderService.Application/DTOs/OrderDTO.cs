using OrderService.Core.Entities;

namespace OrderService.Application.DTOs
{
    public class OrderDTO
    {       
        public OrderDTO() { }

        public OrderDTO(Order order)
        {
            Id = order.Id;
            Status = order.Status.ToString();
            TotalCost = order.TotalCost;
            CreatedDateUtc = order.CreatedDateUtc;
            UpdatedDateUtc = order.UpdatedDateUtc;
        }

        public Guid Id { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
        public string Status { get; set; } = "New";
        public decimal TotalCost { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
    }
}
