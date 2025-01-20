using CatalogService.Core.Entities;

namespace CatalogService.Application.Interfaces
{
    public interface ICatalogRepository
    {
        Task CreateProductAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<Product>> GetAllProductsAsync();        
        Task DeleteProductAsync(Product product);
        Task<bool> IsProductUniqueAsync(string name);
        Task SaveChangesAsync();
        Task UpdateProductAsync(Product updateProduct);
    }
}
