using assessment_erionshahini_API.Entities;

namespace Repository.Interface;

public interface IBookmarkRepository
{
    Task<Bookmark> AddAsync(Bookmark bookmark, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bookmark>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bookmark>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Bookmark?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Bookmark bookmark, CancellationToken cancellationToken = default);
    Task DeleteAsync(Bookmark bookmark, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bookmark>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bookmark>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
}
