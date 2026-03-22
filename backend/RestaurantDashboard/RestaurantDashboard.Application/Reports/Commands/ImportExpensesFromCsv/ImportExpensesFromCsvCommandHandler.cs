using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantDashboard.Domain.Entities;
using RestaurantDashboard.Domain.Enums;
using RestaurantDashboard.Domain.Repositories;

namespace RestaurantDashboard.Application.Reports.Commands.ImportExpensesFromCsv;

public sealed class ImportExpensesFromCsvCommandHandler
    : IRequestHandler<ImportExpensesFromCsvCommand, int>
{
    private readonly IExpenseRepository _expenses;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<ImportExpensesFromCsvCommandHandler> _logger;

    public ImportExpensesFromCsvCommandHandler(
        IExpenseRepository expenses,
        IUnitOfWork uow,
        ILogger<ImportExpensesFromCsvCommandHandler> logger)
    {
        _expenses = expenses;
        _uow = uow;
        _logger = logger;
    }

    public async Task<int> Handle(ImportExpensesFromCsvCommand request, CancellationToken cancellationToken)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
        };

        using var stream = new MemoryStream(request.CsvContent);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, config);

        var rows = csv.GetRecords<ExpenseCsvRow>().ToList();
        int count = 0;
        int rowNumber = 1; // 1-based (row 1 = first data row after header)

        foreach (var row in rows)
        {
            rowNumber++;

            if (row.Amount <= 0 || string.IsNullOrWhiteSpace(row.Description))
            {
                _logger.LogWarning("CSV import: skipping row {Row} — invalid amount or empty description.", rowNumber);
                continue;
            }

            if (!Enum.TryParse<ExpenseCategory>(row.Category, ignoreCase: true, out var category))
            {
                _logger.LogWarning(
                    "CSV import: row {Row} has unrecognised category '{Category}'. Defaulting to '{Default}'.",
                    rowNumber, row.Category, ExpenseCategory.Other);
                category = ExpenseCategory.Other;
            }

            var expense = Expense.Record(
                category,
                row.Amount,
                row.Date,
                row.Description.Trim(),
                request.RecordedByEmployeeId);

            await _expenses.AddAsync(expense, cancellationToken);
            count++;
        }

        if (count > 0)
            await _uow.CommitAsync(cancellationToken);

        return count;
    }
}
