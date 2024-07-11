using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalkStream_API.DTO;
using TalkStream_API.Entities;

[ApiController]
[Route("[controller]")]

public class PublicationsController : ControllerBase
{
    private readonly IPublicationRepository _repository;

    public PublicationsController(IPublicationRepository repository)
    {
        _repository = repository;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetPublications()
    {
        var publications = await _repository.GetAllPublicationsAsync();

        var simplifiedPublications = publications.Select(p => new
        {
            id = p.Id,
            userId = p.UserId,
            content = p.Content,
            timestamp = p.Timestamp,
            sender = new
            {
                uid = p.User.Uid,
                username = p.User.Username,
                email = p.User.Email,
                role = p.User.Role
            },
            comments = p.Comments.Select(c => new
            {
                id = c.Id,
                userId = c.UserId,
                user = c.User.Username, // Assuming you want to show the username of the commenter
                publicationId = c.PublicationId,
                content = c.Content,
                timestamp = c.Timestamp
            }).ToList(),
            reactions = p.Reactions.Select(r => new
            {
                id = r.Id,
                userId = r.UserId,
                user = r.User.Username, // Assuming you want to show the username of the reactor
                publicationId = r.PublicationId,
                type = r.Type
            }).ToList()
        }).ToList();

        return Ok(simplifiedPublications);
    }


    [HttpPost]
    public async Task<ActionResult<Publication>> PostPublication(PublicationDto publicationDto)
    {
        var publication = new Publication
        {
            UserId = publicationDto.UserId,
            Content = publicationDto.Content
        };
        await _repository.AddPublicationAsync(publication);
        return Ok(publication);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPublication(int id, Publication publication)
    {
        if (id != publication.Id)
        {
            return BadRequest();
        }

        await _repository.UpdatePublicationAsync(publication);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePublication(int id)
    {
        await _repository.DeletePublicationAsync(id);
        return NoContent();
    }
    
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(int id, CommentDto commentDto)
    {
        var comment = new Comment
        {
            UserId = commentDto.UserId,
            PublicationId = id,
            Content = commentDto.Content,
            Timestamp = DateTime.Now
        };
        await _repository.AddCommentAsync(comment);
        return Ok(comment);
    }

    [HttpPost("{id}/reactions")]
    public async Task<IActionResult> AddReaction(int id, ReactionDto reactionDto)
    {
        var reaction = new Reaction
        {
            UserId = reactionDto.UserId,
            PublicationId = id,
            Type = reactionDto.Type
        };
        await _repository.AddReactionAsync(reaction);
        return Ok(reaction);
    }
}