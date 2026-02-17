using System.ComponentModel.DataAnnotations;

namespace Services.DTOs;

public class CreateAnnotationRequest
{
    public Guid VideoId { get; set; }
    public double TimestampSeconds { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
}
