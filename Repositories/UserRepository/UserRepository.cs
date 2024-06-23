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
    public User? GetUser(string uid)
    {
        return _context.Users.Find(uid);
    }
}