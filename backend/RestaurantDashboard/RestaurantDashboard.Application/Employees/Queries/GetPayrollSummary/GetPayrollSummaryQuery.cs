using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;
using RestaurantDashboard.Application.Employees.Dtos;

namespace RestaurantDashboard.Application.Employees.Queries.GetPayrollSummary;

public sealed record GetPayrollSummaryQuery : IRequest<IReadOnlyList<EmployeePayrollDto>>, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
}
