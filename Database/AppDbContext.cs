using Microsoft.EntityFrameworkCore;
using TalkStream_API.Entities;

namespace TalkStream_API.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    
}