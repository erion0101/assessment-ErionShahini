namespace assessment_erionshahini_Layout.Data;

public class VideoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public long? FileSizeBytes { get; set; }
    public Guid UserId { get; set; }
    public string? UploaderName { get; set; }
    public string? UploaderEmail { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? StreamUrl { get; set; }
}
