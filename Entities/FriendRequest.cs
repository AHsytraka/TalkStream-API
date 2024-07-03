using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class FriendRequest
{
    [Key]
    public Guid Id { get; set; }
    
    public string SenderId { get; set; }
    public User Sender { get; set; }
    
    public string ReceiverId { get; set; }
    public User Receiver { get; set; }
    
    public DateTime RequestedOn { get; set; }
    public bool? Accepted { get; set; }
}