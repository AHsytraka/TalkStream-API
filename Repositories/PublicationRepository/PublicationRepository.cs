using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.PublicationRepository;

public class PublicationRepository : IPublicationRepository
{
    private readonly AppDbContext _context;

    public PublicationRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Publication>> GetAllPublicationsAsync()
    {
        return await _context.Publications
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.Reactions)
            .ToListAsync();
    }

    public async Task<Publication> GetPublicationByIdAsync(int id)
    {
        return await _context.Publications
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.Reactions)
            .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException();
    }

    public async Task AddPublicationAsync(Publication publication)
    {
        _context.Publications.Add(publication);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePublicationAsync(Publication publication)
    {
        _context.Entry(publication).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletePublicationAsync(int id)
    {
        var publication = await _context.Publications.FindAsync(id);
        if (publication != null)
        {
            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task AddCommentAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task AddReactionAsync(Reaction reaction)
    {
        _context.Reactions.Add(reaction);
        await _context.SaveChangesAsync();
    }
}