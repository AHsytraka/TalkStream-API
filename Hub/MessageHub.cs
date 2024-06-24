using Microsoft.AspNetCore.SignalR;

namespace TalkStream_API.Hub;

public class MessagingHub : Microsoft.AspNetCore.SignalR.Hub
{
    private static readonly List<UserMessage> MessageHistory = new List<UserMessage>();
    public async Task PostMessage(string content)
    {
        var senderId = Context.ConnectionId;
        var userMessage = new UserMessage
        {
            Sender = senderId,
            Content = content,
            SentTime = DateTime.UtcNow
        };
        MessageHistory.Add(userMessage);
        await Clients.Others.SendAsync("receivemessage", senderId, content, userMessage.SentTime);
    }
    public async Task RetrieveMessageHistory() => 
        await Clients.Caller.SendAsync("messagehistory", MessageHistory);
}

public class UserMessage
{
    public required string Sender { get; set; }
    public required string Content { get; set; }
    public DateTime SentTime { get; set; }
}
