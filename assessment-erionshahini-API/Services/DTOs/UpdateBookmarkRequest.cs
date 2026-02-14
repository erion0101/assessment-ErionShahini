namespace Services.DTOs;

public class UpdateBookmarkRequest
{
    public double TimestampSeconds { get; set; }
    public string Title { get; set; } = string.Empty;
}
