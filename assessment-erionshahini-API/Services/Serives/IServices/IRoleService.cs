using Microsoft.AspNetCore.Identity;

namespace Services;

public interface IRoleService
{
    Task<IdentityRole<Guid>> AddRoleAsync(string name, CancellationToken cancellationToken = default);
    Task<IdentityRole<Guid>?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
}
