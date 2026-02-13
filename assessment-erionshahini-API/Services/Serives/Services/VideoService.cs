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

    private static VideoResponse ToResponse(Video v)
    {
        return new VideoResponse
        {
            Id = v.Id,
            Title = v.Title,
            ContentType = v.ContentType,
            FileSizeBytes = v.FileSizeBytes,
            UserId = v.UserId,
            UploadedAt = v.UploadedAt,
            StreamUrl = $"/api/Videos/Stream/{v.Id}"
        };
    }
}
