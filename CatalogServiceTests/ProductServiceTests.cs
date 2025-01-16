using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogServiceTests
{
    public class ProductServiceTests
    {
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly Mock<IValidator<ProductDTO>> _productValidatorMock;
        private readonly Mock<ICatalogRepository> _catalogRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _loggerMock = new Mock<ILogger<ProductService>>();
            _productValidatorMock = new Mock<IValidator<ProductDTO>>();
            _catalogRepositoryMock = new Mock<ICatalogRepository>();

            _productService = new ProductService(
                _catalogRepositoryMock.Object,
                _loggerMock.Object,
                _productValidatorMock.Object);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldSucceed_WhenProductIsValid()
        {
            // Arrange
            var validProduct = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Valid Product",
                Description = "This is a valid product description.",
                Category = "Electronics",
                Price = 1000,
                Quantity = 10,
                CreatedDateUtc = DateTime.UtcNow,
                UpdatedDateUtc = DateTime.UtcNow
            };

            _productValidatorMock
                .Setup(validator => validator.ValidateAsync(It.IsAny<ProductDTO>(), default))
                .ReturnsAsync(new ValidationResult());
            
            _catalogRepositoryMock
                .Setup(repo => repo.IsProductUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            await _productService.CreateProductAsync(validProduct);

            // Assert
            _catalogRepositoryMock.Verify(repo => repo.CreateProductAsync(It.IsAny<Product>()), Times.Once);

            _productValidatorMock.Verify(validator => validator.ValidateAsync(It.IsAny<ProductDTO>(), default), Times.Once);
        }
    }
}
