using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.Entities;

namespace TalkStream_API.Hub;

public class MessagingHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly AppDbContext _context;


    public MessagingHub(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task SendMessage(string senderId, string receiverId, string message)
    {
        var newMessage = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = message,
            Timestamp = DateTime.UtcNow
        };
        
        await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, newMessage.Content, newMessage.Timestamp);
        await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, newMessage.Content, newMessage.Timestamp);

        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();
    }


    public async Task GetMessages(string currentUserId, string otherUserId)
    {
        var messages = _context.Messages
            .Where(m => (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                        (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
            .OrderBy(m => m.Timestamp)
            .ToList();

        await Clients.Caller.SendAsync("ReceiveMessageHistory", messages);
    }
}
