using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities;

public class Publication
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    public virtual User User { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
    // Navigation properties
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Reaction> Reactions { get; set; }
}