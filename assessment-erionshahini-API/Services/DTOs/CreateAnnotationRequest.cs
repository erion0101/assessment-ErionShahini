namespace Services.DTOs;

public class CreateAnnotationRequest
{
    public Guid VideoId { get; set; }
    public double TimestampSeconds { get; set; }
    public string Description { get; set; } = string.Empty;
}
