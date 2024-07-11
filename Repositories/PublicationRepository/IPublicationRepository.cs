using System.Collections.Generic;
using System.Threading.Tasks;
using TalkStream_API.Entities;

public interface IPublicationRepository
{
    Task<IEnumerable<Publication>> GetAllPublicationsAsync();
    Task<Publication> GetPublicationByIdAsync(int id);
    Task AddPublicationAsync(Publication publication);
    Task UpdatePublicationAsync(Publication publication);
    Task DeletePublicationAsync(int id);
    Task AddCommentAsync(Comment comment);
    Task AddReactionAsync(Reaction reaction);
    
    
}