using Microsoft.AspNetCore.Identity;
using Repository.Interface;

namespace Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IdentityRole<Guid>> AddRoleAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty.", nameof(name));

        var existing = await _roleRepository.GetByNameAsync(name, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException($"Role '{name}' already exists.");

        return await _roleRepository.AddAsync(name, cancellationToken);
    }

    public async Task<IdentityRole<Guid>?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.GetByNameAsync(name, cancellationToken);
    }

    public async Task<IReadOnlyList<IdentityRole<Guid>>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _roleRepository.GetAllAsync(cancellationToken);
    }
}
