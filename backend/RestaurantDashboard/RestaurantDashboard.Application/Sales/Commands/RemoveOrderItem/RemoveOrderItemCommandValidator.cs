using FluentValidation;

namespace RestaurantDashboard.Application.Sales.Commands.RemoveOrderItem;

public sealed class RemoveOrderItemCommandValidator : AbstractValidator<RemoveOrderItemCommand>
{
    public RemoveOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.OrderItemId).NotEmpty();
    }
}
