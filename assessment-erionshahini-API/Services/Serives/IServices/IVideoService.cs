using Microsoft.AspNetCore.Http;
using Services.DTOs;

namespace Services;

public interface IVideoService
{
    Task<Result<VideoResponse>> UploadAsync(Guid userId, string title, IFormFile file, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VideoResponse>> GetMyVideosAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<VideoResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(string? FullPath, string? ContentType)> GetStreamInfoAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VideoResponse>> GetAllVideosAsync(CancellationToken cancellationToken = default);
    /// <summary>Delete video (and file). Caller must ensure admin/owner. Returns true if deleted.</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
