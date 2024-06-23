using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.UserRepository;

public interface IUserRepository
{
    public User? GetUser(string uid);
    
}