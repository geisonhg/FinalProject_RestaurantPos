using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;
using RestaurantDashboard.Domain.Enums;

namespace RestaurantDashboard.Application.Reports.Commands.GenerateWeeklyReport;

public sealed record GenerateWeeklyReportCommand : IRequest<Guid>, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
    public Guid GeneratedByEmployeeId { get; init; }
    public ReportType Type { get; init; } = ReportType.Weekly;
}
