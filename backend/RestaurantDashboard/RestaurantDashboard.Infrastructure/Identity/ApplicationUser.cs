using Microsoft.AspNetCore.Identity;

namespace RestaurantDashboard.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public Guid? EmployeeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public ApplicationUser()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
