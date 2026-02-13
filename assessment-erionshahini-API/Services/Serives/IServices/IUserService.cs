using Services.DTOs;

namespace Services;

public interface IUserService
{
    Task<Result<AuthResult>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task LogoutAsync(CancellationToken cancellationToken = default);
    Task<Result<AuthResult>> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken = default);
}
