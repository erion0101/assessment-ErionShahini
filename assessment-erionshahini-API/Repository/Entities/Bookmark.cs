namespace assessment_erionshahini_API.Entities;

public class Bookmark
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
    /// <summary>Timestamp in seconds from video start.</summary>
    public double TimestampSeconds { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Video Video { get; set; } = null!;
    public User User { get; set; } = null!;
}
