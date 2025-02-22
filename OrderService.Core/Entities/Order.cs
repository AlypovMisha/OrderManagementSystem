namespace OrderService.Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public OrderStatus Status { get; set; } = OrderStatus.New;
        public decimal TotalCost { get; set; }
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDateUtc { get; set; } = DateTime.UtcNow;
    }
}
