using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public User RegisterUser(User user)
    {
        _context.Users.Add(user);
        var save = _context.SaveChanges();
        if (save != 0)
            return user; 
        throw new InvalidOperationException();
    }

    public User GetUserById(string uid)
    {
        return _context.Users.Find(uid);
    }

    public User GetUserByUsername(string username)
    {
        return _context.Users.Find(username);
    }

    public User GetUserByEmail(string email)
    {
        return _context.Users.Find(email);
    }

    public User GetUserByUsernameOrEmail(string usernameOrEmail)
    {
        return _context.Users.FirstOrDefault(u => 
            u.Userame == usernameOrEmail || 
            u.Email == usernameOrEmail) ?? throw new BadHttpRequestException("Wrong username or password");
    }
}