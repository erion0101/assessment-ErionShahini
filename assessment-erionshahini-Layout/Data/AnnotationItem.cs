namespace assessment_erionshahini_Layout.Data;

public class AnnotationItem
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
    public double TimestampSeconds { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateAnnotationRequest
{
    public Guid VideoId { get; set; }
    public double TimestampSeconds { get; set; }
    public string Description { get; set; } = string.Empty;
}
