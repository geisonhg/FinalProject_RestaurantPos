using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;

namespace RestaurantDashboard.Application.Expenses.Commands.ApproveExpense;

public sealed record ApproveExpenseCommand : IRequest<Unit>, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
    public Guid ExpenseId { get; init; }
}
