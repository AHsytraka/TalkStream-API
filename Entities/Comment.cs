using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    public virtual User User { get; set; }
    
    [Required]
    public int PublicationId { get; set; }
    public virtual Publication Publication { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public DateTime Timestamp { get; set; }
}