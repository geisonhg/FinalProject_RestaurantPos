using RestaurantDashboard.Domain.Common;
using RestaurantDashboard.Domain.Enums;
using RestaurantDashboard.Domain.Exceptions;

namespace RestaurantDashboard.Domain.Entities;

public sealed class Shift : Entity
{
    public Guid EmployeeId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly ClockIn { get; private set; }
    public TimeOnly? ClockOut { get; private set; }
    public decimal TipsEarned { get; private set; }
    public ShiftStatus Status { get; private set; }

    public TimeSpan Duration
    {
        get
        {
            if (!ClockOut.HasValue) return TimeSpan.Zero;
            var duration = ClockOut.Value - ClockIn;
            // Handle overnight shifts (e.g. 23:00 → 03:00 next day)
            return duration < TimeSpan.Zero ? duration.Add(TimeSpan.FromHours(24)) : duration;
        }
    }

    private Shift() { }

    internal static Shift StartNow(Guid employeeId)
    {
        var now = DateTime.UtcNow;
        return new Shift
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            Date = DateOnly.FromDateTime(now),
            ClockIn = TimeOnly.FromDateTime(now),
            Status = ShiftStatus.Active
        };
    }

    internal void Close(TimeOnly clockOut, decimal tipsEarned)
    {
        if (Status != ShiftStatus.Active)
            throw new DomainException($"Cannot close a shift with status '{Status}'.");

        // Allow overnight shifts (clockOut < ClockIn means next-day clock-out)
        var tentativeDuration = clockOut - ClockIn;
        if (tentativeDuration < TimeSpan.Zero)
            tentativeDuration = tentativeDuration.Add(TimeSpan.FromHours(24));

        if (tentativeDuration.TotalHours > 16)
            throw new DomainException(
                $"Shift duration exceeds the 16-hour limit ({tentativeDuration.TotalHours:F1}h). Contact a manager to close this shift manually.");

        Guard.AgainstNegative(tipsEarned, nameof(tipsEarned));

        ClockOut = clockOut;
        TipsEarned = tipsEarned;
        Status = ShiftStatus.Completed;
    }
}
