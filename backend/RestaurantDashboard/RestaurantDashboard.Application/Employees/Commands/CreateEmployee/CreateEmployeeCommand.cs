using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;
using RestaurantDashboard.Application.Employees.Dtos;
using RestaurantDashboard.Domain.Enums;

namespace RestaurantDashboard.Application.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand : IRequest<EmployeeDto>, IRequireRole
{
    public string RequiredRole => AppRoles.Admin;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public EmployeeRole Role { get; init; }
    public DateOnly HireDate { get; init; }
}
