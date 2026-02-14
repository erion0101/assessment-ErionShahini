using assessment_erionshahini_API.Entities;
using Repository.Interface;
using Services.DTOs;

namespace Services;

public class AnnotationService : IAnnotationService
{
    private readonly IAnnotationRepository _annotationRepository;
    private readonly IVideoRepository _videoRepository;

    public AnnotationService(IAnnotationRepository annotationRepository, IVideoRepository videoRepository)
    {
        _annotationRepository = annotationRepository;
        _videoRepository = videoRepository;
    }

    public async Task<Result<AnnotationResponse>> CreateAsync(Guid userId, CreateAnnotationRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Description))
            return Result<AnnotationResponse>.Failure("Description is required.");
        if (request.TimestampSeconds < 0)
            return Result<AnnotationResponse>.Failure("TimestampSeconds must be >= 0.");

        var video = await _videoRepository.GetByIdAsync(request.VideoId, cancellationToken);
        if (video == null)
            return Result<AnnotationResponse>.Failure("Video not found.");

        var annotation = new Annotation
        {
            VideoId = request.VideoId,
            UserId = userId,
            TimestampSeconds = request.TimestampSeconds,
            Description = request.Description.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        await _annotationRepository.AddAsync(annotation, cancellationToken);
        return Result<AnnotationResponse>.Success(ToResponse(annotation));
    }

    public async Task<IReadOnlyList<AnnotationResponse>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        var list = await _annotationRepository.GetByVideoIdAsync(videoId, cancellationToken);
        return list.Select(ToResponse).ToList();
    }

    public async Task<IReadOnlyList<AnnotationResponse>> GetMyAnnotationsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var list = await _annotationRepository.GetByUserIdAsync(userId, cancellationToken);
        return list.Select(ToResponse).ToList();
    }

    public async Task<AnnotationResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var annotation = await _annotationRepository.GetByIdAsync(id, cancellationToken);
        return annotation == null ? null : ToResponse(annotation);
    }

    public async Task<Result<AnnotationResponse>> UpdateAsync(Guid id, Guid userId, UpdateAnnotationRequest request, CancellationToken cancellationToken = default)
    {
        var annotation = await _annotationRepository.GetByIdAsync(id, cancellationToken);
        if (annotation == null)
            return Result<AnnotationResponse>.Failure("Annotation not found.");
        if (annotation.UserId != userId)
            return Result<AnnotationResponse>.Failure("You can only update your own annotation.");

        annotation.TimestampSeconds = request.TimestampSeconds;
        annotation.Description = request.Description.Trim();
        await _annotationRepository.UpdateAsync(annotation, cancellationToken);
        return Result<AnnotationResponse>.Success(ToResponse(annotation));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var annotation = await _annotationRepository.GetByIdAsync(id, cancellationToken);
        if (annotation == null)
            return Result<bool>.Failure("Annotation not found.");
        if (annotation.UserId != userId)
            return Result<bool>.Failure("You can only delete your own annotation.");

        await _annotationRepository.DeleteAsync(annotation, cancellationToken);
        return Result<bool>.Success(true);
    }

    public async Task<IReadOnlyList<AnnotationResponse>> GetAllAnnotationsAsync(CancellationToken cancellationToken = default)
    {
        var list = await _annotationRepository.GetAllAsync(cancellationToken);
        return list.Select(ToResponse).ToList();
    }

    private static AnnotationResponse ToResponse(Annotation a)
    {
        return new AnnotationResponse
        {
            Id = a.Id,
            VideoId = a.VideoId,
            UserId = a.UserId,
            TimestampSeconds = a.TimestampSeconds,
            Description = a.Description,
            CreatedAt = a.CreatedAt
        };
    }
}
