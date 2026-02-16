using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace assessment_erionshahini_Layout.Service;

public class StreamTokenService : IStreamTokenService
{
    private readonly string _secret;
    private const int ValidMinutes = 15;

    public StreamTokenService(IConfiguration configuration)
    {
        _secret = configuration["ApiSettings:StreamTokenSecret"]
                  ?? configuration["JwtSettings:Secret"]
                  ?? throw new InvalidOperationException("ApiSettings:StreamTokenSecret or JwtSettings:Secret must be set (same value as API JwtSettings:Secret).");
    }

    public string CreateStreamToken(Guid videoId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new System.Security.Claims.Claim("vid", videoId.ToString()),
            new System.Security.Claims.Claim("stream", "true")
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(ValidMinutes),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
