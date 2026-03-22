using MediatR;
using RestaurantDashboard.Application.Common.Exceptions;
using RestaurantDashboard.Application.Common.Interfaces;
using RestaurantDashboard.Application.Employees.Dtos;
using RestaurantDashboard.Application.Sales.Dtos;
using RestaurantDashboard.Domain.Entities;
using RestaurantDashboard.Domain.Repositories;

namespace RestaurantDashboard.Application.Reports.Commands.GenerateWeeklyReport;

/// <summary>
/// Handles GenerateWeeklyReportInternalCommand (background job use only).
/// Shares the same core logic as GenerateWeeklyReportCommandHandler.
/// </summary>
public sealed class GenerateWeeklyReportInternalCommandHandler
    : IRequestHandler<GenerateWeeklyReportInternalCommand, Guid>
{
    private readonly ISaleRepository _sales;
    private readonly IExpenseRepository _expenses;
    private readonly IReportRepository _reports;
    private readonly IReportService _reportService;
    private readonly IEmployeeRepository _employees;
    private readonly IUnitOfWork _uow;

    public GenerateWeeklyReportInternalCommandHandler(
        ISaleRepository sales,
        IExpenseRepository expenses,
        IReportRepository reports,
        IReportService reportService,
        IEmployeeRepository employees,
        IUnitOfWork uow)
    {
        _sales = sales;
        _expenses = expenses;
        _reports = reports;
        _reportService = reportService;
        _employees = employees;
        _uow = uow;
    }

    public async Task<Guid> Handle(GenerateWeeklyReportInternalCommand request, CancellationToken cancellationToken)
    {
        _ = await _employees.GetByIdAsync(request.GeneratedByEmployeeId, cancellationToken)
            ?? throw new NotFoundException(nameof(Employee), request.GeneratedByEmployeeId);

        var saleRecords = await _sales.GetByDateRangeAsync(request.From, request.To, cancellationToken);
        var totalExpenses = await _expenses.GetTotalAsync(request.From, request.To, cancellationToken);
        var expenseRecords = await _expenses.GetByDateRangeAsync(request.From, request.To, cancellationToken);

        var summary = new SalesSummaryDto
        {
            PeriodStart = request.From,
            PeriodEnd = request.To,
            TotalRevenue = saleRecords.Sum(s => s.TotalAmount.Amount),
            TotalTips = saleRecords.Sum(s => s.TipAmount.Amount),
            TotalTransactions = saleRecords.Count,
            TotalExpenses = totalExpenses,
            RevenueByPaymentMethod = saleRecords
                .GroupBy(s => s.PaymentMethod)
                .ToDictionary(g => g.Key.ToString(), g => g.Sum(s => s.TotalAmount.Amount)),
            TransactionsByPaymentMethod = saleRecords
                .GroupBy(s => s.PaymentMethod)
                .ToDictionary(g => g.Key.ToString(), g => g.Count()),
            ExpensesByCategory = expenseRecords
                .Where(e => e.IsApproved)
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key.ToString(), g => g.Sum(e => e.Amount.Amount))
        };

        var filePath = await _reportService.GenerateWeeklyReportAsync(
            summary,
            Enumerable.Empty<EmployeePerformanceDto>(),
            cancellationToken);

        var report = Report.Create(
            request.Type,
            request.From,
            request.To,
            filePath,
            request.GeneratedByEmployeeId);

        await _reports.AddAsync(report, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        return report.Id;
    }
}
