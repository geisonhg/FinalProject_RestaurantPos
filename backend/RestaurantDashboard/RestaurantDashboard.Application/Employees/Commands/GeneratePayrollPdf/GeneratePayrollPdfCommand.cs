using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;

namespace RestaurantDashboard.Application.Employees.Commands.GeneratePayrollPdf;

public sealed record GeneratePayrollPdfCommand : IRequest<string>, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
}
