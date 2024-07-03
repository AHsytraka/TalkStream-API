using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class User
{
    [Key]
    [MaxLength(36)]
    public required string Uid { get; set; }
    
    [MaxLength(50)]
    public required string Userame { get; set; }
    [MaxLength(255)]
    public required string Email { get; set; }
    [MaxLength(120)]
    public required string Password { get; set; }
    public Role Role { get; set; }
    
    public ICollection<User> Friends { get; set; }
    public ICollection<FriendRequest> SentFriendRequests { get; set; }
    public ICollection<FriendRequest> ReceivedFriendRequests { get; set; }
}