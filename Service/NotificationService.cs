using TalkStream_API.Database;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories;

public class NotificationService
{
    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task SendNotificationAsync(string userId, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // Here you would also integrate with SignalR or another real-time messaging system to send the notification to the user if they are online.
    }
}