using Microsoft.AspNet.SignalR.Messaging;
using TalkStream_API.DTO;
using Message = TalkStream_API.Entities.Message;

namespace TalkStream_API.Repositories.MessageRepository;

public interface IMessageRepository
{
    Task<IEnumerable<MessageWithReceiverName>> GetConversationAsync(string senderId, string receiverId);
    Task<IEnumerable<ConversationDto>> GetUserConversationsAsync(string userId);

}