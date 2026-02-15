using Microsoft.AspNetCore.Identity;
using assessment_erionshahini_API.Entities;

namespace Repository.Interface;

public interface IRegisterRepository
{
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IdentityResult> CreateUserAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<IdentityRole<Guid>?> FindRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<IdentityRole<Guid>?> FindRoleByNameAsync(string roleName, CancellationToken cancellationToken = default);
    Task<IdentityRole<Guid>?> CreateRoleAsync(string roleName, CancellationToken cancellationToken = default);
    Task<IdentityResult> AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default);
    Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default);
    Task<IdentityResult> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
}
