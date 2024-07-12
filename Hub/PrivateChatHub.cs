using Microsoft.AspNetCore.SignalR;
using TalkStream_API.Database;
using TalkStream_API.Entities;
using TalkStream_API.Helpers;

namespace TalkStream_API.Hub;

public class PrivateChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly AppDbContext _context;
    
    public PrivateChatHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string senderId, string receiverId, string username, string message)
    {
        var timestamp = DateTime.UtcNow;
        var chatMessage = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = message,
            Timestamp = timestamp
        };

        _context.Messages.Add(chatMessage);
        await _context.SaveChangesAsync();

        var groupName = GroupHelper.GetGroupName(senderId, receiverId);
        await Clients.Group(groupName).SendAsync("ReceiveMessage", senderId, username, message, timestamp.ToString("o"));
    }

    public async Task JoinChat(string senderId, string receiverId)
    {
        var groupName = GroupHelper.GetGroupName(senderId, receiverId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveChat(string senderId, string receiverId)
    {
        var groupName = GroupHelper.GetGroupName(senderId, receiverId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}