using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class User
{
    [Key]
    [MaxLength(36)]
    public required string Uid { get; set; }
    
    [MaxLength(50)]
    public required string Username { get; set; }
    [MaxLength(255)]
    public required string Email { get; set; }
    [MaxLength(120)]
    public required string Password { get; set; }
    public Role Role { get; set; }
    // Navigation properties
    public virtual ICollection<Friendship> Friends { get; set; }
    public virtual ICollection<Friendship> FriendOf { get; set; }
    public virtual ICollection<Publication> Publications { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Reaction> Reactions { get; set; }
}