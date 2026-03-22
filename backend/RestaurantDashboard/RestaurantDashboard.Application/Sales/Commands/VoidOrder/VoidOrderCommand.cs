using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;

namespace RestaurantDashboard.Application.Sales.Commands.VoidOrder;

public sealed record VoidOrderCommand : IRequest<Unit>, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
    public Guid OrderId { get; init; }
    public string Reason { get; init; } = default!;
}
