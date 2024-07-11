using Microsoft.EntityFrameworkCore;
using TalkStream_API.Entities;

namespace TalkStream_API.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Publication> Publications { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity
        modelBuilder.Entity<User>()
            .HasKey(u => u.Uid);

        // Message entity
        modelBuilder.Entity<Message>()
            .HasKey(m => m.Id);

        // Friendship entity
        modelBuilder.Entity<Friendship>()
            .HasKey(f => f.Id);

        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.Requester)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.RequesterId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.Addressee)
            .WithMany(u => u.FriendOf)
            .HasForeignKey(f => f.AddresseeId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // Publication entity
        modelBuilder.Entity<Publication>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Publication>()
            .HasOne(p => p.User)
            .WithMany(u => u.Publications)
            .HasForeignKey(p => p.UserId);

        // Comment entity
        modelBuilder.Entity<Comment>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Publication)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PublicationId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);

        // Reaction entity
        modelBuilder.Entity<Reaction>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.Publication)
            .WithMany(p => p.Reactions)
            .HasForeignKey(r => r.PublicationId);

        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reactions)
            .HasForeignKey(r => r.UserId);
    }
}