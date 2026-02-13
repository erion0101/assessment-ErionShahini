using assessment_erionshahini_API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Services;

public interface IJwtService
{
    string GenerateToken(User user, IList<string> roles);
    string GenerateRefreshToken();
}
