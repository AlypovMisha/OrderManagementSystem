namespace OrderService.Core.Entities
{
    public enum OrderStatus
    {
        New,
        Cancelled,
        InAssembly,
        Assembled,
        InDelivery,
        Delivered
    }
}