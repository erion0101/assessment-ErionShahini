using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;

namespace assessment_erionshahini_API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VideosController : ControllerBase
{
    private readonly IVideoService _videoService;

    public VideosController(IVideoService videoService)
    {
        _videoService = videoService;
    }

    /// <summary>Upload a video (multipart). Only authenticated user.</summary>
    [HttpPost]
    [Route("[action]")]
    [RequestSizeLimit(500_000_000)] // 500 MB
    [ProducesResponseType(typeof(VideoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Upload([FromForm] UploadVideoRequest request, CancellationToken cancellationToken)
    {
        if (request?.File == null) return BadRequest("File is required.");

        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var result = await _videoService.UploadAsync(userId.Value, request.Title, request.File, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetMyVideos(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var list = await _videoService.GetMyVideosAsync(userId.Value, cancellationToken);
        return Ok(list);
    }

    /// <summary>Get one video metadata and stream URL.</summary>
    [HttpGet]
    [Route("[action]/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var video = await _videoService.GetByIdAsync(id, cancellationToken);
        if (video == null) return NotFound();
        return Ok(video);
    }

    /// <summary>Stream or download the video file. Can open this URL in browser to watch.</summary>
    [HttpGet]
    [Route("[action]/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Stream(Guid id, CancellationToken cancellationToken)
    {
        var (fullPath, contentType) = await _videoService.GetStreamInfoAsync(id, cancellationToken);
        if (fullPath == null) return NotFound();
        return PhysicalFile(fullPath, contentType ?? "application/octet-stream", enableRangeProcessing: true);
    }

    /// <summary>Delete a video (and its file). Owner or admin.</summary>
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var callerId = GetCurrentUserId();
        if (callerId == null) return Unauthorized();

        var video = await _videoService.GetByIdAsync(id, cancellationToken);
        if (video == null) return NotFound();

        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin && video.UserId != callerId.Value)
            return Forbid();

        var deleted = await _videoService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }

    /// <summary>Same logic as api/auth/Me. With MapInboundClaims=false, the claim is "sub".</summary>
    private Guid? GetCurrentUserId()
    {
        var id = User.FindFirstValue("sub")
                  ?? User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        return Guid.TryParse(id, out var guid) ? guid : null;
    }
}
