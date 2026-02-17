using System.ComponentModel.DataAnnotations;

namespace Services.DTOs;

public class CreateBookmarkRequest
{
    public Guid VideoId { get; set; }
    public double TimestampSeconds { get; set; }

    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
}
