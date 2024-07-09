using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class Friend
{
    [Key]
    public required string Uid { get; set; }
    
    public required string Username { get; set; }
    
    public required string Email { get; set; }
}