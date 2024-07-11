using System.Collections;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.UserRepository;

public interface IUserRepository
{
    public User RegisterUser(User user);
    public User GetUserById(string uid);
    public Task<User> GetUserByIdAsync(string uid);

    
    public User GetUserByUsername(string username);

    public User GetUserByEmail(string email);

    public User GetUserByUsernameOrEmail(string usernameOrEmail);

    public IEnumerable<User> GetUsersByUsername(string username);

    public IEnumerable<User> GetUsersWithUsername(string username);
}