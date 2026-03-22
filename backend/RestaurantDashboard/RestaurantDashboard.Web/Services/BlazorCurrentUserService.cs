using Microsoft.AspNetCore.Components.Authorization;
using RestaurantDashboard.Application.Common.Interfaces;
using System.Security.Claims;

namespace RestaurantDashboard.Web.Services;

/// <summary>
/// Blazor Server implementation of ICurrentUserService.
/// Resolves the current user from the AuthenticationStateProvider circuit context.
/// </summary>
public sealed class BlazorCurrentUserService : ICurrentUserService
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public BlazorCurrentUserService(AuthenticationStateProvider authStateProvider) =>
        _authStateProvider = authStateProvider;

    private ClaimsPrincipal? _cachedUser;

    private async Task<ClaimsPrincipal> GetUserAsync()
    {
        if (_cachedUser is not null) return _cachedUser;
        var state = await _authStateProvider.GetAuthenticationStateAsync();
        _cachedUser = state.User;
        return _cachedUser;
    }

    public Guid? UserId
    {
        get
        {
            var user = GetUserAsync().GetAwaiter().GetResult();
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string? UserName
    {
        get
        {
            var user = GetUserAsync().GetAwaiter().GetResult();
            return user.FindFirstValue(ClaimTypes.Name);
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            var user = GetUserAsync().GetAwaiter().GetResult();
            return user.Identity?.IsAuthenticated ?? false;
        }
    }

    public bool IsInRole(string role)
    {
        var user = GetUserAsync().GetAwaiter().GetResult();
        return user.IsInRole(role);
    }
}
