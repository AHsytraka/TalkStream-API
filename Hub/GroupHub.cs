using Microsoft.AspNetCore.SignalR;
using TalkStream_API.Database;
using TalkStream_API.Entities;

namespace TalkStream_API.Hub;

public class GroupHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly AppDbContext _context;
    
    public GroupHub(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task SendMessage(string groupId, string userId, string username, string message, string timestamp)
    {
        await Clients.Group(groupId).SendAsync("ReceiveMessage", userId, username, message, timestamp);
    
        // Save message to database
        var groupMessage = new GroupMessage
        {
            GroupId = int.Parse(groupId),
            SenderId = userId,
            Content = message,
            Timestamp = DateTime.Parse(timestamp)
        };

        _context.GroupMessages.Add(groupMessage);
        await _context.SaveChangesAsync();
    }



    public async Task JoinGroup(string groupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }

    public async Task LeaveGroup(string groupId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
    }
}