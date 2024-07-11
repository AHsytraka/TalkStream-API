using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class Friendship
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string RequesterId { get; set; }
    public virtual User Requester { get; set; }
    
    [Required]
    public string AddresseeId { get; set; }
    public virtual User Addressee { get; set; }
    
    public bool IsAccepted { get; set; }
}