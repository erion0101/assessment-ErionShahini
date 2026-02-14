using Services.DTOs;

namespace Services;

public interface IUserService
{
    Task<Result<AuthResult>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task LogoutAsync(CancellationToken cancellationToken = default);
    /// <summary>Invalidate refresh token and clear it from DB. Used when logout is triggered from browser (cookie only).</summary>
    Task LogoutByRefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken = default);
    Task<Result<AuthResult>> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken = default);
}
