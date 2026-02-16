using Microsoft.EntityFrameworkCore;
using assessment_erionshahini_API.Entities;
using Repository.dbContext;
using Repository.Interface;

namespace Repository;

public class VideoRepository : IVideoRepository
{
    private readonly ApplicationDbContext _context;

    public VideoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Video> AddAsync(Video video, CancellationToken cancellationToken = default)
    {
        _context.Videos.Add(video);
        await _context.SaveChangesAsync(cancellationToken);
        return video;
    }

    public async Task<IReadOnlyList<Video>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Videos
            .Include(v => v.User)
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.UploadedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Videos
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Video>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Videos
            .Include(v => v.User)
            .OrderByDescending(v => v.UploadedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Video video, CancellationToken cancellationToken = default)
    {
        _context.Videos.Remove(video);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
