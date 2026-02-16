using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Services;

public class StreamTokenValidator : IStreamTokenValidator
{
    private readonly JwtSettings _settings;

    public StreamTokenValidator(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public bool TryValidate(string? bearerToken, Guid videoId, out string? error)
    {
        error = null;
        if (string.IsNullOrWhiteSpace(bearerToken))
        {
            error = "Missing token";
            return false;
        }

        var token = bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? bearerToken["Bearer ".Length..].Trim()
            : bearerToken.Trim();

        if (string.IsNullOrEmpty(token))
        {
            error = "Empty token";
            return false;
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, parameters, out _);
            var vidClaim = principal.FindFirst("vid")?.Value;
            if (string.IsNullOrEmpty(vidClaim) || !Guid.TryParse(vidClaim, out var vid) || vid != videoId)
            {
                error = "Invalid video id in token";
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}
