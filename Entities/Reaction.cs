using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class Reaction
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    public virtual User User { get; set; }
    
    [Required]
    public int PublicationId { get; set; }
    public virtual Publication Publication { get; set; }
    
    public string Type { get; set; }
}