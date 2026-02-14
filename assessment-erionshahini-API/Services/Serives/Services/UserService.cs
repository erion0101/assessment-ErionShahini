using System.Security.Claims;
using Repository.Interface;
using Services.DTOs;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        IUserRepository userRepository,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<AuthResult>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);
        if (user == null)
            return Result<AuthResult>.Failure("Invalid credentials.");

        var passwordValid = await _userRepository.CheckPasswordAsync(user, request.Password, cancellationToken);
        if (!passwordValid)
            return Result<AuthResult>.Failure("Invalid credentials.");

        var roles = await _userRepository.GetRolesAsync(user, cancellationToken);
        var userRole = roles.FirstOrDefault();
        if (string.IsNullOrEmpty(userRole))
            return Result<AuthResult>.Failure("Invalid credentials.");

        var accessToken = _jwtService.GenerateToken(user, roles);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? null;
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        if (!string.IsNullOrEmpty(ip))
            user.RefreshTokenIp = ip.Length > 50 ? ip[..50] : ip;

        await _userRepository.UpdateUserAsync(user, cancellationToken);

        var authResult = new AuthResult(true, $"Login successful - Role: {userRole}", accessToken, refreshToken);
        return Result<AuthResult>.Success(authResult);
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return;

        var user = await _userRepository.FindByIdAsync(userId, cancellationToken);
        if (user == null)
            return;

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        user.RefreshTokenIp = null;
        await _userRepository.UpdateUserAsync(user, cancellationToken);
    }

    public async Task LogoutByRefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return;

        var user = await _userRepository.FindByRefreshTokenAsync(refreshToken, cancellationToken);
        if (user == null)
            return;

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        user.RefreshTokenIp = null;
        await _userRepository.UpdateUserAsync(user, cancellationToken);
    }

    public async Task<Result<AuthResult>> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return Result<AuthResult>.Failure("Refresh token is missing.");

        var user = await _userRepository.FindByRefreshTokenAsync(refreshToken, cancellationToken);
        if (user == null)
            return Result<AuthResult>.Failure("Invalid or expired refresh token.");

        var roles = await _userRepository.GetRolesAsync(user, cancellationToken);
        var userRole = roles.FirstOrDefault() ?? "User";

        var accessToken = _jwtService.GenerateToken(user, roles);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        if (!string.IsNullOrEmpty(ip))
            user.RefreshTokenIp = ip.Length > 50 ? ip[..50] : ip;

        await _userRepository.UpdateUserAsync(user, cancellationToken);

        var authResult = new AuthResult(true, "Token refreshed.", accessToken, newRefreshToken);
        return Result<AuthResult>.Success(authResult);
    }
}
