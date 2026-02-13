using assessment_erionshahini_API.Entities;
using Microsoft.AspNetCore.Identity;
using Repository.Interface;
using Services.DTOs;

namespace Services;

public class RegisterService : IRegisterService
{
    private readonly IRegisterRepository _registerRepository;
    private readonly IJwtService _jwtService;

    public RegisterService(IRegisterRepository registerRepository, IJwtService jwtService)
    {
        _registerRepository = registerRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResult>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _registerRepository.FindByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
            return Result<AuthResult>.Failure("Email already exists.");

        var role = await _registerRepository.FindRoleByIdAsync(request.RoleId, cancellationToken);
        if (role == null)
            return Result<AuthResult>.Failure("Role not found.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await _registerRepository.CreateUserAsync(user, request.Password, cancellationToken);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            return Result<AuthResult>.Failure(errors);
        }

        var addRoleResult = await _registerRepository.AddToRoleAsync(user, role.Name!, cancellationToken);
        if (!addRoleResult.Succeeded)
        {
            var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
            return Result<AuthResult>.Failure(errors);
        }

        var roles = await _registerRepository.GetRolesAsync(user, cancellationToken);
        var accessToken = _jwtService.GenerateToken(user, roles);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _registerRepository.UpdateUserAsync(user, cancellationToken);

        var authResult = new AuthResult(true, "User registered successfully.", accessToken, refreshToken);
        return Result<AuthResult>.Success(authResult);
    }
}
