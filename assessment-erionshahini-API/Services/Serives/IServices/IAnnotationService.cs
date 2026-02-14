using Services.DTOs;

namespace Services;

public interface IAnnotationService
{
    Task<Result<AnnotationResponse>> CreateAsync(Guid userId, CreateAnnotationRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AnnotationResponse>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AnnotationResponse>> GetMyAnnotationsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<AnnotationResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<AnnotationResponse>> UpdateAsync(Guid id, Guid userId, UpdateAnnotationRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AnnotationResponse>> GetAllAnnotationsAsync(CancellationToken cancellationToken = default);
}
