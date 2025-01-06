using CatalogService.Application.DTOs;
using CatalogService.Core.Entities;

namespace CatalogService.Application.Interfaces
{
    public interface ICatalogRepository
    {
        Task CreateProductAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<Product>> GetAllProductsAsync();
        Task<bool> UpdateProductAsync(Guid id, Product updateProduct);
        Task<bool> DeleteByIdAsync(Guid id);
        Task<bool> UpdateQuantityAsync(Guid id, int quantity);
        Task<bool> IsProductUniqueAsync(string name);
    }
}
