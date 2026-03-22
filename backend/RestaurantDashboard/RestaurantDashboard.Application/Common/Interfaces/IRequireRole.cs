namespace RestaurantDashboard.Application.Common.Interfaces;

/// <summary>
/// Marker interface for commands/queries that require a specific role.
/// Enforced by AuthorizationBehavior in the MediatR pipeline.
/// </summary>
public interface IRequireRole
{
    string RequiredRole { get; }
}
