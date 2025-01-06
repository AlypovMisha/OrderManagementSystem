using CatalogService.Application.DTOs;
using CatalogService.Application.Validations;
using FluentValidation.TestHelper;

namespace CatalogServiceTests
{
    public class ProductValidatorTests
    {
        private readonly ProductValidator _validator;

        public ProductValidatorTests()
        {
            _validator = new ProductValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new ProductDTO { Name = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_Is_Valid()
        {
            var model = new ProductDTO { Name = "Valid Name" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Price_Is_Negative()
        {
            var model = new ProductDTO { Price = -10 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
    }
}
