using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;
using RestaurantDashboard.Application.Employees.Dtos;
using RestaurantDashboard.Domain.Repositories;

namespace RestaurantDashboard.Application.Employees.Commands.GeneratePayrollPdf;

public sealed class GeneratePayrollPdfCommandHandler : IRequestHandler<GeneratePayrollPdfCommand, string>
{
    private readonly IEmployeeRepository _employees;
    private readonly IReportService _reportService;

    public GeneratePayrollPdfCommandHandler(IEmployeeRepository employees, IReportService reportService)
    {
        _employees = employees;
        _reportService = reportService;
    }

    public async Task<string> Handle(GeneratePayrollPdfCommand request, CancellationToken cancellationToken)
    {
        var employees = await _employees.GetAllActiveWithShiftsAsync(cancellationToken);

        var rows = employees
            .Select(e => new EmployeePayrollDto
            {
                EmployeeId = e.Id,
                FullName = e.FullName,
                Role = e.Role.ToString(),
                ShiftCount = e.GetTotalShifts(request.From, request.To),
                TotalHours = e.GetTotalHours(request.From, request.To),
                TotalTips = e.GetTotalTips(request.From, request.To)
            })
            .Where(e => e.ShiftCount > 0)
            .OrderByDescending(e => e.TotalHours)
            .ToList();

        return await _reportService.GeneratePayrollReportAsync(rows, request.From, request.To, cancellationToken);
    }
}
