using MediatR;
using RestaurantDashboard.Domain.Enums;

namespace RestaurantDashboard.Application.Reports.Commands.GenerateWeeklyReport;

/// <summary>
/// Internal variant of GenerateWeeklyReportCommand used exclusively by background jobs.
/// Does NOT implement IRequireRole — it bypasses the AuthorizationBehavior intentionally
/// since background services run without an authenticated user context.
/// Do NOT expose this command through any UI endpoint.
/// </summary>
public sealed record GenerateWeeklyReportInternalCommand : IRequest<Guid>
{
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
    public Guid GeneratedByEmployeeId { get; init; }
    public ReportType Type { get; init; } = ReportType.Weekly;
}
