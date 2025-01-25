using CatalogService.Application.DTOs;

namespace CatalogService.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProductAsync(ProductDTO productDto);
        Task DeleteProductAsync(Guid id);
        Task<ProductDTO> GetProductByIdAsync(Guid id);
        Task<ProductDTO> UpdateProductAsync(Guid id, ProductDTO productDTO);
        Task UpdateQuantityProductAsync(Guid id, int quantity);
    }
}
