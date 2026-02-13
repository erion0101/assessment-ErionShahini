using Microsoft.AspNetCore.Http;

namespace Services.DTOs;

public class UploadVideoRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
}
