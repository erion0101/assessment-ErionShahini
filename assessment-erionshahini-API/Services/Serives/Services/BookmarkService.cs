using assessment_erionshahini_API.Entities;
using Repository.Interface;
using Services.DTOs;

namespace Services;

public class BookmarkService : IBookmarkService
{
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly IVideoRepository _videoRepository;

    public BookmarkService(IBookmarkRepository bookmarkRepository, IVideoRepository videoRepository)
    {
        _bookmarkRepository = bookmarkRepository;
        _videoRepository = videoRepository;
    }

    public async Task<Result<BookmarkResponse>> CreateAsync(Guid userId, CreateBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<BookmarkResponse>.Failure("Title is required.");
        if (request.TimestampSeconds < 0)
            return Result<BookmarkResponse>.Failure("TimestampSeconds must be >= 0.");

        var video = await _videoRepository.GetByIdAsync(request.VideoId, cancellationToken);
        if (video == null)
            return Result<BookmarkResponse>.Failure("Video not found.");

        var bookmark = new Bookmark
        {
            VideoId = request.VideoId,
            UserId = userId,
            TimestampSeconds = request.TimestampSeconds,
            Title = request.Title.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        await _bookmarkRepository.AddAsync(bookmark, cancellationToken);
        return Result<BookmarkResponse>.Success(ToResponse(bookmark));
    }

    public async Task<IReadOnlyList<BookmarkResponse>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        var list = await _bookmarkRepository.GetByVideoIdAsync(videoId, cancellationToken);
        return list.Select(ToResponse).ToList();
    }

    public async Task<IReadOnlyList<BookmarkResponse>> GetMyBookmarksAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var list = await _bookmarkRepository.GetByUserIdAsync(userId, cancellationToken);
        return list.Select(ToResponse).ToList();
    }

    public async Task<BookmarkResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var bookmark = await _bookmarkRepository.GetByIdAsync(id, cancellationToken);
        return bookmark == null ? null : ToResponse(bookmark);
    }

    public async Task<Result<BookmarkResponse>> UpdateAsync(Guid id, Guid userId, UpdateBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var bookmark = await _bookmarkRepository.GetByIdAsync(id, cancellationToken);
        if (bookmark == null)
            return Result<BookmarkResponse>.Failure("Bookmark not found.");
        if (bookmark.UserId != userId)
            return Result<BookmarkResponse>.Failure("You can only update your own bookmark.");

        bookmark.TimestampSeconds = request.TimestampSeconds;
        bookmark.Title = request.Title.Trim();
        await _bookmarkRepository.UpdateAsync(bookmark, cancellationToken);
        return Result<BookmarkResponse>.Success(ToResponse(bookmark));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var bookmark = await _bookmarkRepository.GetByIdAsync(id, cancellationToken);
        if (bookmark == null)
            return Result<bool>.Failure("Bookmark not found.");
        if (bookmark.UserId != userId)
            return Result<bool>.Failure("You can only delete your own bookmark.");

        await _bookmarkRepository.DeleteAsync(bookmark, cancellationToken);
        return Result<bool>.Success(true);
    }

    public async Task<IReadOnlyList<BookmarkResponse>> GetAllBookmarksAsync(CancellationToken cancellationToken = default)
    {
        var list = await _bookmarkRepository.GetAllAsync(cancellationToken);
        return list.Select(ToResponse).ToList();
    }

    private static BookmarkResponse ToResponse(Bookmark b)
    {
        return new BookmarkResponse
        {
            Id = b.Id,
            VideoId = b.VideoId,
            UserId = b.UserId,
            TimestampSeconds = b.TimestampSeconds,
            Title = b.Title,
            CreatedAt = b.CreatedAt
        };
    }
}
