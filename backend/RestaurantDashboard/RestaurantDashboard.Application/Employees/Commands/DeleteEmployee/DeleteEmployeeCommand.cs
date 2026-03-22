using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;

namespace RestaurantDashboard.Application.Employees.Commands.DeleteEmployee;

public sealed record DeleteEmployeeCommand(Guid EmployeeId) : IRequest, IRequireRole
{
    public string RequiredRole => AppRoles.Admin;
}
