using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.UserRepository;

public interface IUserRepository
{
    public User RegisterUser(User user);
    public User GetUserById(string uid);
    
    public User GetUserByUsername(string username);

    public User GetUserByEmail(string email);

    public User GetUserByUsernameOrEmail(string usernameOrEmail);

    public IEnumerable<User> GetUsersByUsername(string username);
}