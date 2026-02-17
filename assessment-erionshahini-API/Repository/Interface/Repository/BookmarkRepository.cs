using Microsoft.EntityFrameworkCore;
using assessment_erionshahini_API.Entities;
using Repository.dbContext;
using Repository.Interface;

namespace Repository;

public class BookmarkRepository : IBookmarkRepository
{
    private readonly ApplicationDbContext _context;

    public BookmarkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Bookmark> AddAsync(Bookmark bookmark, CancellationToken cancellationToken = default)
    {
        _context.Bookmarks.Add(bookmark);
        await _context.SaveChangesAsync(cancellationToken);
        return bookmark;
    }

    public async Task<IReadOnlyList<Bookmark>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookmarks
            .Where(b => b.VideoId == videoId)
            .OrderBy(b => b.TimestampSeconds)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Bookmark>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookmarks
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Bookmark?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Bookmarks
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Bookmark bookmark, CancellationToken cancellationToken = default)
    {
        _context.Bookmarks.Update(bookmark);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Bookmark bookmark, CancellationToken cancellationToken = default)
    {
        _context.Bookmarks.Remove(bookmark);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Bookmark>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Bookmarks
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Bookmark>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Bookmarks
            .Include(b => b.Video)
            .Include(b => b.User)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
