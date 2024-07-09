using Microsoft.AspNetCore.SignalR;
using TalkStream_API.Entities;

namespace TalkStream_API.Hub;

public class MessagingHub : Microsoft.AspNetCore.SignalR.Hub
{
    private static readonly List<UserMessage> MessageHistory = [];
    public async Task PostMessage(string content)
    {
        var senderId = Context.ConnectionId;
        var userMessage = new UserMessage
        {
            SenderId = senderId,
            Content = content,
            SentTime = DateTime.UtcNow
        };
        MessageHistory.Add(userMessage);
        await Clients.Others.SendAsync("receivemessage", senderId, content, userMessage.SentTime);
    }
    public async Task RetrieveMessageHistory() => 
        await Clients.Caller.SendAsync("messagehistory", MessageHistory);
}


