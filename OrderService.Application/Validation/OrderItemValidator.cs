using FluentValidation;
using OrderService.Application.DTOs;

namespace OrderService.Application.Validation
{
    public class OrderItemValidator : AbstractValidator<OrderItemDTO>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.Quantity)
               .GreaterThanOrEqualTo(1).WithMessage("Quantity must be at least 1.");
        }
    }
}
