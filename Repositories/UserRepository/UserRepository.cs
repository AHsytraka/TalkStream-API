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
        return _context.Users.FirstOrDefault(u => u.Uid == uid);
    }

    public async Task<User> GetUserByIdAsync(string uid)
    {
        return await _context.Users.FindAsync(uid);
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
            u.Username == usernameOrEmail || 
            u.Email == usernameOrEmail) ?? throw new BadHttpRequestException("Wrong username or password");
    }

    public IEnumerable<User> GetUsersByUsername(string username)
    {
        return _context.Users.Where(u => u.Username.Contains(username));
    }

    public IEnumerable<User> GetUsersWithUsername(string username)
    {
        return _context.Users.Where(u => u.Username == username);
    }

    public async Task AddFriendAsync(string userId, string friendId)
    {
        var user = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Uid == userId);
        var friend = await _context.Users.FindAsync(friendId);

        if (user != null && friend != null && !user.Friends.Contains(friend))
        {
            user.Friends.Add(friend);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SendFriendRequestAsync(string requesterId, string addresseeId)
    {
        var friendRequest = new FriendRequest
        {
            RequesterId = requesterId,
            AddresseeId = addresseeId
        };

        _context.FriendRequests.Add(friendRequest);
        await _context.SaveChangesAsync();
    }

    public async Task RespondToFriendRequestAsync(string requestId, bool isAccepted)
    {
        var request = await _context.FriendRequests.FindAsync(requestId);
        if (request != null)
        {
            request.IsAccepted = isAccepted;
            if (isAccepted)
            {
                // Add to each other's friend list if accepted
                var user = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Uid == request.RequesterId);
                var friend = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Uid == request.AddresseeId);
                user?.Friends.Add(friend);
                friend?.Friends.Add(user);
            }
            await _context.SaveChangesAsync();
        }    }
}