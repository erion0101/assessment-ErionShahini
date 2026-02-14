using assessment_erionshahini_API.Entities;

namespace Repository.Interface;

public interface IAnnotationRepository
{
    Task<Annotation> AddAsync(Annotation annotation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Annotation>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Annotation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Annotation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Annotation annotation, CancellationToken cancellationToken = default);
    Task DeleteAsync(Annotation annotation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Annotation>> GetAllAsync(CancellationToken cancellationToken = default);
}
