using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;

namespace RestaurantDashboard.Application.Reports.Commands.ImportExpensesFromCsv;

public sealed record ImportExpensesFromCsvCommand : IRequest<int>, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
    /// <summary>Raw bytes of the uploaded CSV file.</summary>
    public byte[] CsvContent { get; init; } = Array.Empty<byte>();

    /// <summary>Employee who is performing the import.</summary>
    public Guid RecordedByEmployeeId { get; init; }
}
