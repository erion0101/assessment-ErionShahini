using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using assessment_erionshahini_API.Entities;
using Repository.Interface;
using Services.DTOs;

namespace Services;

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository;
    private readonly IWebHostEnvironment _env;
    private const string UploadSubPath = "uploads/videos";

    public VideoService(IVideoRepository videoRepository, IWebHostEnvironment env)
    {
        _videoRepository = videoRepository;
        _env = env;
    }

    public async Task<Result<VideoResponse>> UploadAsync(Guid userId, string title, IFormFile file, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<VideoResponse>.Failure("Title is required.");
        if (file == null || file.Length == 0)
            return Result<VideoResponse>.Failure("A non-empty file is required.");

        var id = Guid.NewGuid();
        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension))
            extension = ".bin";

        var allowedExtensions = new[] { ".mp4", ".webm", ".mov", ".avi" };
        if (!allowedExtensions.Contains(extension.ToLowerInvariant()))
            return Result<VideoResponse>.Failure("Only video files (.mp4, .webm, .mov, .avi) are allowed.");
        var relativePath = Path.Combine(UploadSubPath, userId.ToString(), $"{id}{extension}").Replace('\\', '/');
        var fullPath = Path.Combine(_env.ContentRootPath, relativePath);

        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        await using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            await file.CopyToAsync(stream, cancellationToken);

        var video = new Video
        {
            Id = id,
            Title = title.Trim(),
            FilePath = relativePath,
            ContentType = file.ContentType,
            FileSizeBytes = file.Length,
            UserId = userId,
            UploadedAt = DateTime.UtcNow
        };
        await _videoRepository.AddAsync(video, cancellationToken);

        var response = ToResponse(video);
        return Result<VideoResponse>.Success(response);
    }

    public async Task<IReadOnlyList<VideoResponse>> GetMyVideosAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var list = await _videoRepository.GetByUserIdAsync(userId, cancellationToken);
        return list.Select(ToResponse).ToList();
    }

    public async Task<VideoResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var video = await _videoRepository.GetByIdAsync(id, cancellationToken);
        return video == null ? null : ToResponse(video);
    }

    public async Task<(string? FullPath, string? ContentType)> GetStreamInfoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var video = await _videoRepository.GetByIdAsync(id, cancellationToken);
        if (video == null) return (null, null);
        var fullPath = Path.Combine(_env.ContentRootPath, video.FilePath);
        if (!System.IO.File.Exists(fullPath)) return (null, null);
        return (fullPath, video.ContentType ?? "application/octet-stream");
    }

    public async Task<IReadOnlyList<VideoResponse>> GetAllVideosAsync(CancellationToken cancellationToken = default)
    {
        var list = await _videoRepository.GetAllAsync(cancellationToken);
        return list.Select(ToResponse).ToList();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var video = await _videoRepository.GetByIdAsync(id, cancellationToken);
        if (video == null) return false;

        var fullPath = Path.Combine(_env.ContentRootPath, video.FilePath);
        if (System.IO.File.Exists(fullPath))
        {
            try { System.IO.File.Delete(fullPath); } catch { /* best effort */ }
        }

        await _videoRepository.DeleteAsync(video, cancellationToken);
        return true;
    }

    private static VideoResponse ToResponse(Video v)
    {
        var uploaderEmail = v.User?.Email;
        var uploaderName = BuildDisplayNameFromEmail(uploaderEmail);

        return new VideoResponse
        {
            Id = v.Id,
            Title = v.Title,
            ContentType = v.ContentType,
            FileSizeBytes = v.FileSizeBytes,
            UserId = v.UserId,
            UploaderName = uploaderName,
            UploaderEmail = uploaderEmail,
            UploadedAt = v.UploadedAt,
            StreamUrl = $"/api/Videos/Stream/{v.Id}"
        };
    }

    private static string BuildDisplayNameFromEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return "Unknown user";

        var localPart = email.Split('@')[0];
        if (string.IsNullOrWhiteSpace(localPart))
            return email;

        var parts = localPart
            .Replace('_', ' ')
            .Replace('.', ' ')
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
            return email;

        return string.Join(" ", parts.Select(p => char.ToUpperInvariant(p[0]) + p[1..]));
    }
}
