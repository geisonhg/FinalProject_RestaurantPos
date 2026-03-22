using MediatR;
using RestaurantDashboard.Application.Common.Interfaces;

namespace RestaurantDashboard.Application.Expenses.Commands.DeleteExpense;

public sealed record DeleteExpenseCommand(Guid ExpenseId) : IRequest, IRequireRole
{
    public string RequiredRole => AppRoles.Manager;
}
