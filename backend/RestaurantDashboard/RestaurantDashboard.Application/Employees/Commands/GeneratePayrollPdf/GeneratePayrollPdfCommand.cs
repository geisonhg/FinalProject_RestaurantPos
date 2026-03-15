using MediatR;

namespace RestaurantDashboard.Application.Employees.Commands.GeneratePayrollPdf;

public sealed record GeneratePayrollPdfCommand : IRequest<string>
{
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
}
