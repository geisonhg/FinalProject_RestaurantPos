using FluentValidation;

namespace RestaurantDashboard.Application.MenuItems.Commands.CreateMenuItem;

public sealed class CreateMenuItemCommandValidator : AbstractValidator<CreateMenuItemCommand>
{
    public CreateMenuItemCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description is not null);
        RuleFor(x => x.BasePrice).GreaterThan(0).WithMessage("Base price must be greater than zero.");
        RuleFor(x => x.StockQuantity).GreaterThan(0).When(x => x.StockQuantity.HasValue)
            .WithMessage("Stock quantity must be greater than zero.");
    }
}
