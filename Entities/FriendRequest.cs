using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class FriendRequest
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RequesterId { get; set; }
    public User Requester { get; set; }
    public string AddresseeId { get; set; }
    public User Addressee { get; set; }
    public bool? IsAccepted { get; set; }
}