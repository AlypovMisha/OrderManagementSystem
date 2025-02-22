namespace OrderService.Application.DTOs
{
    public class OrderItemDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
