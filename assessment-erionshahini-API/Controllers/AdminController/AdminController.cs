using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public AdminController(
        IVideoService videoService,
        IAnnotationService annotationService,
        IBookmarkService bookmarkService)
    {
        _videoService = videoService;
        _annotationService = annotationService;
        _bookmarkService = bookmarkService;
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

    /// <summary>All bookmarks (Admin only).</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetBookmarks(CancellationToken cancellationToken)
    {
        var list = await _bookmarkService.GetAllBookmarksAsync(cancellationToken);
        return Ok(list);
    }
}
