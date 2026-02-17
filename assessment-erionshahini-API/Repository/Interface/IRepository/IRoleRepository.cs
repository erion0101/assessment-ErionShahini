using Microsoft.AspNetCore.Identity;

namespace Repository.Interface;

public interface IRoleRepository
{
    Task<IdentityRole<Guid>> AddAsync(string name, CancellationToken cancellationToken = default);
    Task<IdentityRole<Guid>?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<IdentityRole<Guid>>> GetAllAsync(CancellationToken cancellationToken = default);
}
