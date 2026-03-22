using FluentValidation;
using RestaurantDashboard.Domain.Repositories;

namespace RestaurantDashboard.Application.Expenses.Commands.RecordExpense;

public sealed class RecordExpenseCommandValidator : AbstractValidator<RecordExpenseCommand>
{
    public RecordExpenseCommandValidator(IEmployeeRepository employees)
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Expense date cannot be in the future.");
        RuleFor(x => x.RecordedByEmployeeId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await employees.GetByIdAsync(id, ct) is not null)
            .WithMessage("The specified employee does not exist.");
        RuleFor(x => x.ReceiptUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .When(x => x.ReceiptUrl is not null)
            .WithMessage("Receipt URL must be a valid absolute URL.");
    }
}
