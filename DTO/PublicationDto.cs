using System.ComponentModel.DataAnnotations;
using TalkStream_API.Entities;

namespace TalkStream_API.DTO;

public record PublicationDto(string UserId, string Content);

public class CommentDto
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Content { get; set; }
}

public class ReactionDto
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Type { get; set; }
}