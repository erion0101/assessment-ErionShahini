using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Services;
using Services.DTOs;

namespace assessment_erionshahini_API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdminController : ControllerBase
{
    private readonly IVideoService _videoService;
    private readonly IAnnotationService _annotationService;
    private readonly IBookmarkService _bookmarkService;
    private readonly IUserRepository _userRepository;
    private readonly IRegisterService _registerService;
    private readonly IRoleService _roleService;

    public AdminController(
        IVideoService videoService,
        IAnnotationService annotationService,
        IBookmarkService bookmarkService,
        IUserRepository userRepository,
        IRegisterService registerService,
        IRoleService roleService)
    {
        _videoService = videoService;
        _annotationService = annotationService;
        _bookmarkService = bookmarkService;
        _userRepository = userRepository;
        _registerService = registerService;
        _roleService = roleService;
    }

    /// <summary>Get all roles for admin create-user form.</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetAllRolesAsync(cancellationToken);
        var list = roles.Select(r => new { id = r.Id, name = r.Name }).ToList();
        return Ok(list);
    }

    /// <summary>Create a new user with the specified role (Admin only).</summary>
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserRequest request, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByNameAsync(request.RoleName?.Trim() ?? "User", cancellationToken);
        if (role == null)
            return BadRequest($"Role '{request.RoleName}' not found.");

        var registerRequest = new RegisterRequest
        {
            Email = request.Email,
            Password = request.Password,
            RoleId = role.Id
        };

        var result = await _registerService.RegisterAsync(registerRequest, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(new { message = "User created successfully.", email = request.Email, role = request.RoleName });
    }

    /// <summary>Dashboard stats: counts for videos, annotations, bookmarks, users.</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
    {
        // DbContext is not thread-safe; run sequentially to avoid "second operation started" error
        var videos = await _videoService.GetAllVideosAsync(cancellationToken);
        var annotations = await _annotationService.GetAllAnnotationsAsync(cancellationToken);
        var bookmarks = await _bookmarkService.GetAllBookmarksAsync(cancellationToken);
        var userCount = await _userRepository.GetCountAsync(cancellationToken);

        var stats = new
        {
            videos = videos.Count,
            annotations = annotations.Count,
            bookmarks = bookmarks.Count,
            users = userCount
        };
        return Ok(stats);
    }

    /// <summary>All videos (Admin only).</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetVideos(CancellationToken cancellationToken)
    {
        var list = await _videoService.GetAllVideosAsync(cancellationToken);
        return Ok(list);
    }

    /// <summary>All annotations (Admin only).</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetAnnotations(CancellationToken cancellationToken)
    {
        var list = await _annotationService.GetAllAnnotationsAsync(cancellationToken);
        return Ok(list);
    }

    /// <summary>All annotations with video title and user name (Admin only).</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetAnnotationsWithDetails(CancellationToken cancellationToken)
    {
        var list = await _annotationService.GetAllAnnotationsWithDetailsAsync(cancellationToken);
        return Ok(list);
    }

    /// <summary>All bookmarks (Admin only).</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetBookmarks(CancellationToken cancellationToken)
    {
        var list = await _bookmarkService.GetAllBookmarksAsync(cancellationToken);
        return Ok(list);
    }

    /// <summary>All bookmarks with video title and user name (Admin only).</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetBookmarksWithDetails(CancellationToken cancellationToken)
    {
        var list = await _bookmarkService.GetAllBookmarksWithDetailsAsync(cancellationToken);
        return Ok(list);
    }
}
