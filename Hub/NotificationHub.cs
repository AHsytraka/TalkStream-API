using Microsoft.AspNetCore.SignalR;

namespace TalkStream_API.Hub;

public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("receivenotification", message);
    }
}