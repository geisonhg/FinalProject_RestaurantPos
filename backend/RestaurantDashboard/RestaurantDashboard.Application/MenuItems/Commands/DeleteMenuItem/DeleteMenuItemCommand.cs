using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;

namespace RestaurantDashboard.Application.MenuItems.Commands.DeleteMenuItem;

public sealed record DeleteMenuItemCommand(Guid MenuItemId) : IRequest, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
}
