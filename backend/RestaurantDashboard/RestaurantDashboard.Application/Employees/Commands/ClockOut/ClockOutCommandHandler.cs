using MediatR;
using RestaurantDashboard.Application.Common.Exceptions;
using RestaurantDashboard.Domain.Entities;
using RestaurantDashboard.Domain.Repositories;

namespace RestaurantDashboard.Application.Employees.Commands.ClockOut;

public sealed class ClockOutCommandHandler : IRequestHandler<ClockOutCommand, Unit>
{
    private readonly IEmployeeRepository _employees;
    private readonly IUnitOfWork _uow;

    public ClockOutCommandHandler(IEmployeeRepository employees, IUnitOfWork uow)
    {
        _employees = employees;
        _uow = uow;
    }

    public async Task<Unit> Handle(ClockOutCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employees.GetByIdWithShiftsAsync(request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException(nameof(Employee), request.EmployeeId);

        var activeShift = employee.Shifts.FirstOrDefault(s => s.Id == request.ShiftId)
            ?? throw new NotFoundException("Shift", request.ShiftId);

        var shiftStart = activeShift.Date.ToDateTime(activeShift.ClockIn);
        var duration = DateTime.UtcNow - shiftStart;
        if (duration.TotalHours > 16)
            throw new InvalidOperationException(
                $"Shift duration exceeds 16 hours ({duration.TotalHours:F1}h). Please contact a manager to close this shift manually.");

        employee.ClockOut(request.ShiftId, request.TipsEarned);

        _employees.Update(employee);
        await _uow.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
