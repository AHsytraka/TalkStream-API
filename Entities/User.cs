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
    public virtual ICollection<Friend> Friends { get; set; } = new List<Friend>();
    public virtual ICollection<FriendRequest> SentFriendRequests { get; set; } = new List<FriendRequest>();
    public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = new List<FriendRequest>();
}