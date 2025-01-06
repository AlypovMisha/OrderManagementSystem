using CatalogService.Application.DTOs;
using CatalogService.Application.Exceptions;
using CatalogService.Application.Interfaces;
using CatalogService.Core.Entities;

namespace CatalogService.Application.Services
{
    public class ProductService(ICatalogRepository catalogRepository) : IProductService
    {
        public async Task<bool> CreateProductAsync(ProductDTO productDto)
        {
            if (!await catalogRepository.IsProductUniqueAsync(productDto.Name))
            {
                throw new ExceptionNameAlreadyExists();
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                Category = productDto.Category,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            await catalogRepository.CreateProductAsync(product);
            return true;
        }

        public async Task<bool> UpdateProductAsync(Guid id, ProductDTO productDto)
        {
            var product = new Product()
            {
                Id = id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                Category = productDto.Category,
                UpdatedDateUtc = DateTime.UtcNow
            };

            return await catalogRepository.UpdateProductAsync(id, product);
        }
    }
}
