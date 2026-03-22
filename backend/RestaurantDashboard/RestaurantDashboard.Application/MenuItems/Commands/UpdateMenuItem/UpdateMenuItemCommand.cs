using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;
using RestaurantDashboard.Application.MenuItems.Dtos;

namespace RestaurantDashboard.Application.MenuItems.Commands.UpdateMenuItem;

public sealed record UpdateMenuItemCommand : IRequest<MenuItemDto>, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
    public Guid MenuItemId { get; init; }
    public string Name { get; init; } = default!;
    public string Category { get; init; } = default!;
    public string? Description { get; init; }
    public decimal BasePrice { get; init; }
    public bool IsAvailable { get; init; }
    public int? StockQuantity { get; init; }
}
