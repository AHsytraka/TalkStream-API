using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.FriendRepository;

public class FriendRepository : IFriendRepository
{
    private readonly AppDbContext _context;
    private readonly NotificationService _notificationService;

    public FriendRepository(AppDbContext context, NotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task SendFriendRequestAsync(string senderId, string receiverId)
    {
        var friendRequest = new FriendRequest
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            RequestedOn = DateTime.UtcNow,
            Accepted = null
        };

        _context.FriendRequests.Add(friendRequest);
        await _context.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(receiverId, "You have a new friend request!");
    }

    public async Task AcceptFriendRequestAsync(Guid requestId)
    {
        var request = await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.Id == requestId);

        if (request != null && request.Accepted == null)
        {
            request.Accepted = true;
            await _context.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(request.SenderId, "Your friend request has been accepted!");
        }
    }

    public async Task DeclineFriendRequestAsync(Guid requestId)
    {
        var request = await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.Id == requestId);

        if (request != null && request.Accepted == null)
        {
            request.Accepted = false;
            await _context.SaveChangesAsync();

            // Optionally send a notification for declined request
        }
    }
}