using MediatR;

namespace RestaurantDashboard.Application.Sales.Commands.RemoveOrderItem;

public sealed record RemoveOrderItemCommand : IRequest<Unit>
{
    public Guid OrderId { get; init; }
    public Guid OrderItemId { get; init; }
}
