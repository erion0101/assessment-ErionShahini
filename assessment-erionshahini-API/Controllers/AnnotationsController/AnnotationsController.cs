using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;

namespace assessment_erionshahini_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AnnotationsController : ControllerBase
{
    private readonly IAnnotationService _annotationService;

    public AnnotationsController(IAnnotationService annotationService)
    {
        _annotationService = annotationService;
    }

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(AnnotationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateAnnotationRequest request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var result = await _annotationService.CreateAsync(userId.Value, request, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpGet]
    [Route("[action]/{videoId:guid}")]
    public async Task<IActionResult> GetByVideo(Guid videoId, CancellationToken cancellationToken)
    {
        var list = await _annotationService.GetByVideoIdAsync(videoId, cancellationToken);
        return Ok(list);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetMyAnnotations(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var list = await _annotationService.GetMyAnnotationsAsync(userId.Value, cancellationToken);
        return Ok(list);
    }

    [HttpGet]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var annotation = await _annotationService.GetByIdAsync(id, cancellationToken);
        if (annotation == null) return NotFound();
        return Ok(annotation);
    }

    [HttpPut]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAnnotationRequest request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var allowAdminBypass = User.IsInRole("Admin");
        var result = await _annotationService.UpdateAsync(id, userId.Value, request, allowAdminBypass, cancellationToken);
        if (!result.IsSuccess)
            return result.Error == "Annotation not found." ? NotFound() : BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpDelete]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var allowAdminBypass = User.IsInRole("Admin");
        var result = await _annotationService.DeleteAsync(id, userId.Value, allowAdminBypass, cancellationToken);
        if (!result.IsSuccess)
            return result.Error == "Annotation not found." ? NotFound() : BadRequest(result.Error);

        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        var id = User.FindFirstValue("sub")
                  ?? User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        return Guid.TryParse(id, out var guid) ? guid : null;
    }
}
