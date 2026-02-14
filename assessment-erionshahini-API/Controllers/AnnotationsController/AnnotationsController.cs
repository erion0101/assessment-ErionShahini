using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;

namespace assessment_erionshahini_API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class AnnotationsController : ControllerBase
{
    private readonly IAnnotationService _annotationService;

    public AnnotationsController(IAnnotationService annotationService)
    {
        _annotationService = annotationService;
    }

    /// <summary>Create annotation for a video.</summary>
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(AnnotationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateAnnotationRequest request, CancellationToken cancellationToken)
    {
        //var userId = GetCurrentUserId();
        //if (userId == null) return Unauthorized();
        Guid guid = Guid.Parse("695B235C-F6D2-4EC9-A60B-EC2F19CEA460");

        var result = await _annotationService.CreateAsync(guid, request, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>List annotations for a video (by videoId).</summary>
    [HttpGet]
    [Route("[action]/{videoId:guid}")]
    public async Task<IActionResult> GetByVideo(Guid videoId, CancellationToken cancellationToken)
    {
        var list = await _annotationService.GetByVideoIdAsync(videoId, cancellationToken);
        return Ok(list);
    }

    /// <summary>List annotations of the current user.</summary>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetMyAnnotations(CancellationToken cancellationToken)
    {
        //var userId = GetCurrentUserId();
        //if (userId == null) return Unauthorized();
        Guid guid = Guid.Parse("695B235C-F6D2-4EC9-A60B-EC2F19CEA460");

        var list = await _annotationService.GetMyAnnotationsAsync(guid, cancellationToken);
        return Ok(list);
    }

    /// <summary>Get one annotation by id.</summary>
    [HttpGet]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var annotation = await _annotationService.GetByIdAsync(id, cancellationToken);
        if (annotation == null) return NotFound();
        return Ok(annotation);
    }

    /// <summary>Update annotation (only owner).</summary>
    [HttpPut]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAnnotationRequest request, CancellationToken cancellationToken)
    {
        //var userId = GetCurrentUserId();
        //if (userId == null) return Unauthorized();
        Guid guid = Guid.Parse("695B235C-F6D2-4EC9-A60B-EC2F19CEA460");

        var result = await _annotationService.UpdateAsync(id, guid, request, cancellationToken);
        if (!result.IsSuccess)
            return result.Error == "Annotation not found." ? NotFound() : BadRequest(result.Error);

        return Ok(result.Data);
    }

    /// <summary>Delete annotation (only owner).</summary>
    [HttpDelete]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        //var userId = GetCurrentUserId();
        //if (userId == null) return Unauthorized();
        Guid guid = Guid.Parse("695B235C-F6D2-4EC9-A60B-EC2F19CEA460");
        var result = await _annotationService.DeleteAsync(id, guid, cancellationToken);
        if (!result.IsSuccess)
            return result.Error == "Annotation not found." ? NotFound() : BadRequest(result.Error);

        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");
        return Guid.TryParse(id, out var guid) ? guid : null;
    }
}
