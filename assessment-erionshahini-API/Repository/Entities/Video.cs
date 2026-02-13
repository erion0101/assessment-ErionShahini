namespace assessment_erionshahini_API.Entities;

public class Video
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    /// <summary>Stored file name on disk (e.g. GUID + extension) or relative path.</summary>
    public string FilePath { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public long? FileSizeBytes { get; set; }
    public Guid UserId { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Annotation> Annotations { get; set; } = new List<Annotation>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
}
