using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.DTO;
using Message = TalkStream_API.Entities.Message;

namespace TalkStream_API.Repositories.MessageRepository;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MessageWithReceiverName>> GetConversationAsync(string senderId, string receiverId)
    {
        var messagesWithReceiverName = await _context.Messages
            .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) || (m.SenderId == receiverId && m.ReceiverId == senderId))
            .Join(_context.Users, // Assuming Users is the DbSet for user entities
                message => message.ReceiverId, 
                user => user.Uid, // Assuming Id is the key in Users entity
                (message, user) => new MessageWithReceiverName
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    Content = message.Content,
                    Timestamp = message.Timestamp,
                    ReceiverName = user.Username 
                })
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        return messagesWithReceiverName;
    }
    
    
    public async Task<IEnumerable<ConversationDto>> GetUserConversationsAsync(string userId)
    {
        var conversations = new List<ConversationDto>();

        // Assuming _context is your DbContext and it has a Messages and Users DbSet
        var messages = await _context.Messages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .Include(m => m.Sender) // Assuming you have navigation properties
            .Include(m => m.Receiver)
            .ToListAsync();

        var groupedMessages = messages
            .GroupBy(m => new { MinId = m.SenderId.CompareTo(m.ReceiverId) <= 0 ? m.SenderId : m.ReceiverId, MaxId = m.SenderId.CompareTo(m.ReceiverId) > 0 ? m.SenderId : m.ReceiverId })
            .ToList();

        foreach (var group in groupedMessages)
        {
            var otherUserId = group.Key.MinId == userId ? group.Key.MaxId : group.Key.MinId;
            var otherUser = await _context.Users.FindAsync(otherUserId);

            conversations.Add(new ConversationDto
            {
                OtherUserId = otherUserId,
                OtherUserName = otherUser?.Username,
                Messages = group.ToList()
            });
        }

        return conversations;
    }
}

public class MessageWithReceiverName : Message
{
    public string ReceiverName { get; set; }
}

