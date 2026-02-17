using Microsoft.EntityFrameworkCore;
using assessment_erionshahini_API.Entities;
using Repository.dbContext;
using Repository.Interface;

namespace Repository;

public class AnnotationRepository : IAnnotationRepository
{
    private readonly ApplicationDbContext _context;

    public AnnotationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Annotation> AddAsync(Annotation annotation, CancellationToken cancellationToken = default)
    {
        _context.Annotations.Add(annotation);
        await _context.SaveChangesAsync(cancellationToken);
        return annotation;
    }

    public async Task<IReadOnlyList<Annotation>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        return await _context.Annotations
            .Where(a => a.VideoId == videoId)
            .OrderBy(a => a.TimestampSeconds)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Annotation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Annotations
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Annotation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Annotations
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Annotation annotation, CancellationToken cancellationToken = default)
    {
        _context.Annotations.Update(annotation);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Annotation annotation, CancellationToken cancellationToken = default)
    {
        _context.Annotations.Remove(annotation);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Annotation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Annotations
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Annotation>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Annotations
            .Include(a => a.Video)
            .Include(a => a.User)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
