namespace assessment_erionshahini_Layout.Data;

public class BookmarkItem
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
    public double TimestampSeconds { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateBookmarkRequest
{
    public Guid VideoId { get; set; }
    public double TimestampSeconds { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class UpdateBookmarkRequest
{
    public double TimestampSeconds { get; set; }
    public string Title { get; set; } = string.Empty;
}
