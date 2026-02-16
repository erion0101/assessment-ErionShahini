using assessment_erionshahini_API.Entities;

namespace Repository.Interface;

public interface IVideoRepository
{
    Task<Video> AddAsync(Video video, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Video>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Video>> GetAllAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Video video, CancellationToken cancellationToken = default);
}
