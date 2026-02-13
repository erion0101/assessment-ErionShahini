using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;
using System.Security.Claims;

namespace assessment_erionshahini_API.Controllers.AuthContoller;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private const string RefreshTokenCookieName = "refreshToken";
    private const int RefreshTokenExpiryDays = 7;

    private readonly IRegisterService _registerService;
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _env;

    public AuthController(IRegisterService registerService, IUserService userService, IWebHostEnvironment env)
    {
        _registerService = registerService;
        _userService = userService;
        _env = env;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _registerService.RegisterAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        SetRefreshTokenCookie(result.Data!.RefreshToken);
        return Ok(CreateAuthResponse(result.Data));
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.LoginAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new AuthResult(false, result.Error ?? "Login failed.", null, null));

        SetRefreshTokenCookie(result.Data!.RefreshToken);
        return Ok(CreateAuthResponse(result.Data));
    }

    [HttpPost]
    [Route("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _userService.LogoutAsync(cancellationToken);
        ClearRefreshTokenCookie();
        return Ok(new { message = "Logged out successfully." });
    }

    /// <summary>Refresh access token using the refresh token from cookie. Call this when access token expires (e.g. on 401).</summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        var result = await _userService.RefreshTokenAsync(refreshToken, cancellationToken);

        if (!result.IsSuccess)
            return Unauthorized(new { message = result.Error });

        SetRefreshTokenCookie(result.Data!.RefreshToken);
        return Ok(CreateAuthResponse(result.Data));
    }

    /// <summary>Current user info from JWT (id, email, roles). Requires valid access token. GET api/auth/Me</summary>
    [HttpGet]
    [Route("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Me()
    {
        var id = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");
        var email = User.FindFirstValue(System.Security.Claims.ClaimTypes.Email)
                    ?? User.FindFirstValue("email");
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
        if (string.IsNullOrEmpty(id))
            return Unauthorized();
        return Ok(new { id, email, roles });
    }

    private void SetRefreshTokenCookie(string? refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return;

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(RefreshTokenExpiryDays)
        };
        Response.Cookies.Append(RefreshTokenCookieName, refreshToken, options);
    }

    private void ClearRefreshTokenCookie()
    {
        Response.Cookies.Append(RefreshTokenCookieName, string.Empty, new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddYears(-1)
        });
    }

    /// <summary>Response without refresh token (it is in HttpOnly cookie). Client should store only AccessToken in memory.</summary>
    private static AuthResult CreateAuthResponse(AuthResult data) =>
        new(data.Success, data.Message, data.AccessToken, null);
}
