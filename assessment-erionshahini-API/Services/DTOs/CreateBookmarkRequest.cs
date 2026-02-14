namespace Services.DTOs;

public class CreateBookmarkRequest
{
    public Guid VideoId { get; set; }
    public double TimestampSeconds { get; set; }
    public string Title { get; set; } = string.Empty;
}
