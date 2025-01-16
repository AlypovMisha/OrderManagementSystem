using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Application.Validations;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogServiceTests
{
    public class CatalogRepositoryTests
    {
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly Mock<IValidator<ProductDTO>> _productValidatorMock;
        private readonly Mock<ICatalogRepository> _catalogRepositoryMock;
        private readonly ProductService _productService;

        public CatalogRepositoryTests()
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
        public async Task NeedToRename()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
