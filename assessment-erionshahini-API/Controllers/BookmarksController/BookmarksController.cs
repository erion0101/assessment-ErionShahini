using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;

namespace assessment_erionshahini_API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class BookmarksController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarksController(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    /// <summary>Create bookmark for a video.</summary>
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(BookmarkResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateBookmarkRequest request, CancellationToken cancellationToken)
    {
        //var userId = GetCurrentUserId();
        //if (userId == null) return Unauthorized();
        Guid guid = Guid.Parse("695B235C-F6D2-4EC9-A60B-EC2F19CEA460");

        var result = await _bookmarkService.CreateAsync(guid, request, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>List bookmarks for a video (by videoId).</summary>
    [HttpGet]
    [Route("[action]/{videoId:guid}")]
    public async Task<IActionResult> GetByVideo(Guid videoId, CancellationToken cancellationToken)
    {
        var list = await _bookmarkService.GetByVideoIdAsync(videoId, cancellationToken);
        return Ok(list);
    }

    /// <summary>List bookmarks of the current user.</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetMyBookmarks(CancellationToken cancellationToken)
    {
        //var userId = GetCurrentUserId();
        //if (userId == null) return Unauthorized();
        Guid guid = Guid.Parse("695B235C-F6D2-4EC9-A60B-EC2F19CEA460");
        var list = await _bookmarkService.GetMyBookmarksAsync(guid, cancellationToken);
        return Ok(list);
    }

    /// <summary>Get one bookmark by id.</summary>
    [HttpGet]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var bookmark = await _bookmarkService.GetByIdAsync(id, cancellationToken);
        if (bookmark == null) return NotFound();
        return Ok(bookmark);
    }

    /// <summary>Update bookmark (only owner).</summary>
    [HttpPut]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookmarkRequest request, CancellationToken cancellationToken)
    {
        //var userId = GetCurrentUserId();
        //if (userId == null) return Unauthorized();
        Guid guid = Guid.Parse("695B235C-F6D2-4EC9-A60B-EC2F19CEA460");

        var result = await _bookmarkService.UpdateAsync(id, guid, request, cancellationToken);
        if (!result.IsSuccess)
            return result.Error == "Bookmark not found." ? NotFound() : BadRequest(result.Error);

        return Ok(result.Data);
    }

    /// <summary>Delete bookmark (only owner).</summary>
    [HttpDelete]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        //var userId = GetCurrentUserId();
        //if (userId == null) return Unauthorized();
        Guid guid = Guid.Parse("695B235C-F6D2-4EC9-A60B-EC2F19CEA460");

        var result = await _bookmarkService.DeleteAsync(id, guid, cancellationToken);
        if (!result.IsSuccess)
            return result.Error == "Bookmark not found." ? NotFound() : BadRequest(result.Error);

        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");
        return Guid.TryParse(id, out var guid) ? guid : null;
    }
}
