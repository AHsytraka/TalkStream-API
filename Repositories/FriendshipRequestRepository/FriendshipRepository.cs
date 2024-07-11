using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.DTO;
using TalkStream_API.Entities;
using TalkStream_API.Repositories.UserRepository;

namespace TalkStream_API.Repositories.FriendRequestRepository;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;

    public FriendshipRepository(IUserRepository userRepository, AppDbContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }
    
    public async Task<IEnumerable<FriendDto>> GetAllFriendsAsync(string userId)
    {
        var friendsAsRequester = await _context.Friendships
            .Where(f => f.RequesterId == userId && f.IsAccepted)
            .Join(_context.Users, 
                friendship => friendship.AddresseeId, 
                user => user.Uid, 
                (friendship, user) => new FriendDto 
                { 
                    Uid = user.Uid, 
                    Name = user.Username
                })
            .ToListAsync();

        var friendsAsAddressee = await _context.Friendships
            .Where(f => f.AddresseeId == userId && f.IsAccepted)
            .Join(_context.Users, 
                friendship => friendship.RequesterId, 
                user => user.Uid, 
                (friendship, user) => new FriendDto 
                { 
                    Uid = user.Uid, 
                    Name = user.Username
                })
            .ToListAsync();

        return friendsAsRequester.Concat(friendsAsAddressee).Distinct();
    }
    
    public async Task<IEnumerable<Friendship>> GetAllReceivedFriendRequestsAsync(string userId)
    {
        return await _context.Friendships
            .Where(f => f.AddresseeId == userId && !f.IsAccepted)
            .ToListAsync();
    }

    public async Task<Friendship> GetFriendshipAsync(string requesterId, string addresseeId)
    {
        return await _context.Friendships
            .FirstOrDefaultAsync(f => f.RequesterId == requesterId && f.AddresseeId == addresseeId);
    }

    public async Task SendFriendRequestAsync(Friendship friendship)
    {
        _context.Friendships.Add(friendship);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFriendshipAsync(Friendship friendship)
    {
        _context.Friendships.Update(friendship);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteFriendshipAsync(Friendship friendship)
    {
        _context.Friendships.Remove(friendship);
        await _context.SaveChangesAsync();
    }

}