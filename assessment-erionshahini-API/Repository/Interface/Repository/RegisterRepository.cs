using Microsoft.AspNetCore.Identity;
using assessment_erionshahini_API.Entities;
using Repository.Interface;

namespace Repository;

public class RegisterRepository : IRegisterRepository
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RegisterRepository(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityRole<Guid>?> FindRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _roleManager.FindByIdAsync(roleId.ToString());
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default)
    {
        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        return await _userManager.UpdateAsync(user);
    }
}
