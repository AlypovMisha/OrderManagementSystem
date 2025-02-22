using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using System.Net.Http.Json;

namespace OrderService.Infrastructure.HttpClients
{
    public class CatalogServiceClient(HttpClient _client, ILogger<CatalogServiceClient> _logger) : ICatalogServiceClient
    {
        public async Task<decimal> GetProductPriceAsync(Guid productId)
        {
            string uri = $"http://catalog_service:8080/api/Products/{productId}";

            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadFromJsonAsync<ProductDTO>();
                return product?.Price ?? throw new Exception("The price was not found");
            }

            _logger.LogWarning($"Error when receiving the product price {productId}: {response.StatusCode}");
            throw new Exception("Couldn't get the product price");
        }

        public async Task<bool> ReserveProductAsync(Guid productId, int quantity)
        {
            string uri = $"http://catalog_service:8080/api/Products/{productId}/quantity";

            var response = await _client.PatchAsJsonAsync(uri, quantity);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogWarning($"Error when reserving an item {productId}: {response.StatusCode}");
            return false;
        }
    }
}
