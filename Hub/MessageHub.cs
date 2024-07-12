using Microsoft.AspNetCore.SignalR;
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

    public async Task SendMessage(string senderId, string senderUsername, string receiverId, string receiverUsername, string message, DateTime timestamp)
    {
        // Send message to the receiver
        await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, senderUsername, message, timestamp);
        
        // Send message to the sender for real-time update
        await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, senderUsername, message, timestamp);
    }

    public async Task GetMessages(string currentUserId, string otherUserId)
    {
        var messages = _context.Messages
            .Where(m => (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                        (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
            .OrderBy(m => m.Timestamp)
            .Select(m => new 
            {
                m.SenderId,
                m.SenderUsername,
                m.ReceiverId,
                m.ReceiverUsername,
                m.Content,
                m.Timestamp
            })
            .ToList();

        await Clients.Caller.SendAsync("ReceiveMessageHistory", messages);
    }
}



// using System.Collections.Concurrent;
// using Microsoft.AspNetCore.SignalR;
// using Microsoft.EntityFrameworkCore;
// using TalkStream_API.Database;
// using TalkStream_API.Entities;
//
// namespace TalkStream_API.Hub;
//
// public class MessagingHub : Microsoft.AspNetCore.SignalR.Hub
// {
//     private readonly AppDbContext _context;
//
//
//     public MessagingHub(AppDbContext context)
//     {
//         _context = context;
//     }
//     
//     public async Task SendMessage(string senderId, string senderUsername, string receiverId, string receiverUsername, string message)
//     {
//         await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, senderUsername, message, DateTime.UtcNow);
//         await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, senderUsername, message, DateTime.UtcNow);
//         var newMessage = new Message
//         {
//             SenderId = senderId,
//             SenderUsername = senderUsername,
//             ReceiverId = receiverId,
//             ReceiverUsername = receiverUsername,
//             Content = message,
//             Timestamp = DateTime.UtcNow
//         };
//
//         _context.Messages.Add(newMessage);
//         await _context.SaveChangesAsync();
//     }
//
//
//
//     public async Task GetMessages(string currentUserId, string otherUserId)
//     {
//         var messages = _context.Messages
//             .Where(m => (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
//                         (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
//             .OrderBy(m => m.Timestamp)
//             .Select(m => new 
//             {
//                 m.SenderId,
//                 m.SenderUsername,
//                 m.ReceiverId,
//                 m.ReceiverUsername,
//                 m.Content,
//                 m.Timestamp
//             })
//             .ToList();
//
//         await Clients.Caller.SendAsync("ReceiveMessageHistory", messages);
//     }
//
// }
