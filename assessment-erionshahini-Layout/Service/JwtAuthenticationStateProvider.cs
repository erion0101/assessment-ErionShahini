using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using assessment_erionshahini_Layout.ConnetionsWithAPIs;

namespace assessment_erionshahini_Layout.Service;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly TokenStorageService _tokenStorage;
    private readonly ApiAuthService _apiAuthService;
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public JwtAuthenticationStateProvider(TokenStorageService tokenStorage, ApiAuthService apiAuthService)
    {
        _tokenStorage = tokenStorage;
        _apiAuthService = apiAuthService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = _tokenStorage.GetAccessToken();

        if (string.IsNullOrEmpty(token))
        {
            var refreshed = await _apiAuthService.TryRefreshTokenAsync();
            if (refreshed)
                token = _tokenStorage.GetAccessToken();
        }

        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(Anonymous);

        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return new AuthenticationState(Anonymous);

            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                var refreshed = await _apiAuthService.TryRefreshTokenAsync();
                if (refreshed)
                {
                    token = _tokenStorage.GetAccessToken();
                    if (!string.IsNullOrEmpty(token))
                        jwtToken = handler.ReadJwtToken(token);
                    else
                        return new AuthenticationState(Anonymous);
                }
                else
                    return new AuthenticationState(Anonymous);
            }

            var claims = new List<Claim>();
            var roleClaims = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var claim in jwtToken.Claims)
            {
                var claimTypeLower = claim.Type.ToLowerInvariant();
                var isRoleClaim =
                    claim.Type == ClaimTypes.Role ||
                    claim.Type == "role" ||
                    claim.Type == "roles" ||
                    claimTypeLower == "role" ||
                    claimTypeLower == "roles" ||
                    (claimTypeLower.EndsWith("/role") && !claimTypeLower.Contains("email") && !claimTypeLower.Contains("name")) ||
                    (claimTypeLower.EndsWith("/roles") && !claimTypeLower.Contains("email") && !claimTypeLower.Contains("name")) ||
                    (claimTypeLower.Contains("role") && !claimTypeLower.Contains("email") && !claimTypeLower.Contains("name") &&
                     !claimTypeLower.Contains("identifier") && !claimTypeLower.Contains("sub"));

                if (isRoleClaim && !string.IsNullOrWhiteSpace(claim.Value))
                    roleClaims.Add(claim.Value.Trim());
                else
                    claims.Add(claim);
            }

            foreach (var role in roleClaims)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var identity = new ClaimsIdentity(
                claims,
                authenticationType: "jwt",
                nameType: ClaimTypes.Name,
                roleType: ClaimTypes.Role);

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(Anonymous);
        }
    }

    public void NotifyUserAuthentication()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
    }
}
