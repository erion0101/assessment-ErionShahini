namespace Services.DTOs;

public class AdminAnnotationResponse
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
    public string VideoTitle { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public double TimestampSeconds { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
