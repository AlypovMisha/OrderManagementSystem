using CatalogService.Application.DTOs;
using CatalogService.Application.Exceptions;
using CatalogService.Application.Interfaces;
using CatalogService.Core.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
namespace CatalogService.Application.Services
{
    public class ProductService(ICatalogRepository _catalogRepository, ILogger<ProductService> _logger, IValidator<ProductDTO> _productValidator) : IProductService
    {
        public async Task<ProductDTO> CreateProductAsync(ProductDTO productDto)
        {
            var validationResult = await _productValidator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Validation Exeption!!!");
                throw new ValidationException(validationResult.Errors);
            }

            if (!await _catalogRepository.IsProductUniqueAsync(productDto.Name))
            {
                _logger.LogWarning($"Product name already exists: {productDto.Name}");
                throw new ExceptionNameAlreadyExists();
            }

            var product = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Category = productDto.Category,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            await _catalogRepository.CreateProductAsync(product);
            return new ProductDTO(product);
        }


        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _catalogRepository.GetByIdAsync(id);

            if (product != null)
            {
                await _catalogRepository.DeleteProductAsync(product);
                await _catalogRepository.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Product with ID {id} is not found");
                throw new ProductNotFoundException($"Product with ID {id} not found.");
            }
        }

        public async Task<ProductDTO> UpdateProductAsync(Guid id, ProductDTO productDto)
        {
            var updateProduct = await _catalogRepository.GetByIdAsync(id);

            if (updateProduct == null)
            {
                _logger.LogWarning($"Product with ID {id} is not found");
                throw new ProductNotFoundException($"Product with ID {id} not found.");
            }

            updateProduct.Name = productDto.Name;
            updateProduct.Description = productDto.Description;
            updateProduct.Category = productDto.Category;
            updateProduct.Price = productDto.Price;
            updateProduct.Quantity = productDto.Quantity;
        

            await _catalogRepository.UpdateProductAsync(updateProduct);
            return new ProductDTO(updateProduct);
        }

        public async Task UpdateQuantityProductAsync(Guid id, int quantity)
        {
            var updateProduct = await _catalogRepository.GetByIdAsync(id);

            if (updateProduct == null)
            {
                _logger.LogWarning($"Product with ID {id} is not found");
                throw new ProductNotFoundException($"Product with ID {id} not found.");
            }
            
            if (updateProduct.Quantity < quantity)
            {
                _logger.LogWarning($"The quantity of the updateProduct is insufficient.");
                throw new QuantityException("Insufficient quantity.");
            }

            updateProduct.Quantity -= quantity;

            await _catalogRepository.UpdateProductAsync(updateProduct);
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            var product = await _catalogRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new ProductNotFoundException($"Product with ID {id} not found.");
            }

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity,
                CreatedDateUtc = product.CreatedDateUtc,
                UpdatedDateUtc = product.UpdatedDateUtc
            };
        }
    }
}
