using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.dbContext;
using Repository.Interface;

namespace Repository;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IdentityRole<Guid>> AddAsync(string name, CancellationToken cancellationToken = default)
    {
        var role = new IdentityRole<Guid>
        {
            Id = Guid.NewGuid(),
            Name = name,
            NormalizedName = name.Normalize().ToUpperInvariant(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync(cancellationToken);
        return role;
    }

    public async Task<IdentityRole<Guid>?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Normalize().ToUpperInvariant();
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
    }
}
