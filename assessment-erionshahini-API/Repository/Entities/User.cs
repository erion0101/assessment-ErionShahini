using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace assessment_erionshahini_API.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public DateTime? UpdatedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    [StringLength(50)]
    public string? RefreshTokenIp { get; set; }

    [StringLength(500)]
    public string? RefreshTokenUserAgent { get; set; }

    public ICollection<Video> Videos { get; set; } = new List<Video>();
    public ICollection<Annotation> Annotations { get; set; } = new List<Annotation>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
}
