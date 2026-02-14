namespace Services.DTOs;

public class UpdateAnnotationRequest
{
    public double TimestampSeconds { get; set; }
    public string Description { get; set; } = string.Empty;
}
