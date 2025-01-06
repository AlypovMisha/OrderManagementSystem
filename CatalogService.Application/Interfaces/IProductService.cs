using CatalogService.Application.DTOs;

namespace CatalogService.Application.Interfaces
{
    public interface IProductService
    {
        Task<bool> UpdateProductAsync(Guid id, ProductDTO productDTO);
        Task<bool> CreateProductAsync(ProductDTO productDto);
    }
}
