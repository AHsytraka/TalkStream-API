using Microsoft.EntityFrameworkCore;
using TalkStream_API.Entities;

namespace TalkStream_API.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        // Configure the primary key for User
        modelBuilder.Entity<User>()
            .HasKey(u => u.Uid);
    
        // Configure the relationship between FriendRequest and User for Requester
        modelBuilder.Entity<FriendRequest>()
            .HasOne(f => f.Requester)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(f => f.RequesterId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust the DeleteBehavior as needed
    
        // Configure the relationship between FriendRequest and User for Addressee
        modelBuilder.Entity<FriendRequest>()
            .HasOne(f => f.Addressee)
            .WithMany(u => u.ReceivedFriendRequests)
            .HasForeignKey(f => f.AddresseeId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust the DeleteBehavior as needed
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserMessage> UserMessages { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
}