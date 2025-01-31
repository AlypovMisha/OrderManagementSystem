namespace OrderService.Core.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}