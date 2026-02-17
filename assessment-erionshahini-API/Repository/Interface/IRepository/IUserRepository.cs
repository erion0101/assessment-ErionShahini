using assessment_erionshahini_API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Repository.Interface;

public interface IUserRepository
{
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> CheckPasswordAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default);
    Task<IdentityResult> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
}
