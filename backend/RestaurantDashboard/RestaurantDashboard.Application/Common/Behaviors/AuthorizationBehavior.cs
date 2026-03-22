using MediatR;
using RestaurantDashboard.Application.Common.Exceptions;
using RestaurantDashboard.Application.Common.Interfaces;

namespace RestaurantDashboard.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that enforces role-based authorization.
/// If a request implements IRequireRole, the current user must have that role.
/// Runs after LoggingBehavior and before ValidationBehavior.
/// </summary>
public sealed class AuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICurrentUserService _currentUser;

    public AuthorizationBehavior(ICurrentUserService currentUser) =>
        _currentUser = currentUser;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IRequireRole requireRole)
            return await next();

        if (!_currentUser.IsAuthenticated)
            throw new ForbiddenException("You must be logged in to perform this action.");

        if (!_currentUser.IsInRole(requireRole.RequiredRole))
            throw new ForbiddenException(
                $"You need the '{requireRole.RequiredRole}' role to perform this action.");

        return await next();
    }
}
