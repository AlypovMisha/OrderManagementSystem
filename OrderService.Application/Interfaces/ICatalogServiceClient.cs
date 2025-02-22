namespace OrderService.Application.Interfaces
{
    public interface ICatalogServiceClient
    {
        public Task<bool> ReserveProductAsync(Guid productId, int quantity);
        public Task<decimal> GetProductPriceAsync(Guid productId);
    }
}
