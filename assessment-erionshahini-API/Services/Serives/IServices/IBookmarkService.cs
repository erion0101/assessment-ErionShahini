using Services.DTOs;

namespace Services;

public interface IBookmarkService
{
    Task<Result<BookmarkResponse>> CreateAsync(Guid userId, CreateBookmarkRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookmarkResponse>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookmarkResponse>> GetMyBookmarksAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<BookmarkResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<BookmarkResponse>> UpdateAsync(Guid id, Guid userId, UpdateBookmarkRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookmarkResponse>> GetAllBookmarksAsync(CancellationToken cancellationToken = default);
}
