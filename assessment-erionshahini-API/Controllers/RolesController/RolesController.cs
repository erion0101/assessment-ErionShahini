using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace assessment_erionshahini_API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize (Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CreateRole([FromBody] string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Role name cannot be empty.");

        var role = await _roleService.AddRoleAsync(name, cancellationToken);
        return CreatedAtAction(nameof(GetByName), new { name = role.Name }, role);
    }

    [HttpGet]
    [Route("[action]/{name}")]
    public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByNameAsync(name, cancellationToken);
        if (role == null)
            return NotFound();
        return Ok(role);
    }
}
