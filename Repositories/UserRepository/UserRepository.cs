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

    public async Task<bool> SendFriendRequestAsync(string requesterId, string addresseeId)
    {
        // Check if a friend request already exists between these users
        var existingRequest = await _context.FriendRequests
            .AnyAsync(fr => (fr.RequesterId == requesterId && fr.AddresseeId == addresseeId) ||
                            (fr.RequesterId == addresseeId && fr.AddresseeId == requesterId));

        if (existingRequest)
        {
            // Friend request already exists, do not create a new one
            return false;
        }

        var friendRequest = new FriendRequest
        {
            RequesterId = requesterId,
            AddresseeId = addresseeId,
            RequesterName = GetUserById(requesterId).Username,
            AddresseeName = GetUserById(addresseeId).Username
        };

        _context.FriendRequests.Add(friendRequest);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> RespondToFriendRequestAsync(string requestId, bool isAccepted)
    {
        var request = await _context.FriendRequests.FindAsync(requestId);
            if ( request != null && isAccepted)
            {
                // Add to each other's friend list if accepted
                var user = _context.Users.Include(u => u.Friends).FirstOrDefault(u => u.Uid == request.RequesterId);
                var userF = new Friend
                {
                    Uid = user.Uid,
                    Username = user.Username,
                    Email = user.Email,
                };
                var friend =  _context.Users.Include(u => u.Friends).FirstOrDefault(u => u.Uid == request.AddresseeId);
                var friendF = new Friend
                {
                    Uid = friend.Uid,
                    Username = friend.Username,
                    Email = friend.Email,
                };
                
                if (user != null && friend != null)
                {
                    user.Friends.Add(friendF);
                    friend.Friends.Add(userF);
                    
                    // Delete the friend request after responding
                    _context.FriendRequests.Remove(request);
                    _context.SaveChanges();

                    return ("Demande accepté");
                }
            }
            
            // Delete the friend request after responding
            _context.FriendRequests.Remove(request);
            await _context.SaveChangesAsync();
            return("Demande refusé");
    }
    
    public async Task<IEnumerable<FriendRequest>> GetSentFriendRequestsAsync(string userId)
    {
        return await _context.FriendRequests
            .Where(fr => fr.RequesterId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<FriendRequest>> GetReceivedFriendRequestsAsync(string userId)
    {
        var fr = await _context.FriendRequests
            .Where(fr => fr.AddresseeId == userId)
            .ToListAsync();

        return fr;
    }

    public async Task<IEnumerable<Friend>> GetUsersFriend(string uid)
    {
        var user = await _context.Users.Include(user => user.Friends).FirstOrDefaultAsync(u => u.Uid == uid);
        return user.Friends;
    }
}