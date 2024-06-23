using Microsoft.AspNetCore.SignalR;
using TalkStream_API.Entities;

namespace TalkStream_API.Hub;

public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IDictionary<string, UserRoomConnection> _connection;
    public async Task JoinRoom(UserRoomConnection userRoomConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userRoomConnection.Room!);
        _connection[Context.ConnectionId] = userRoomConnection;
        await Clients.Group(userRoomConnection.Room!)
            .SendAsync("ReceiveMessage", "Chat manager", $"{userRoomConnection.User} joined the chat");
        await SendConnectedUser(userRoomConnection.Room!);
    }

    public async Task SendMessage(string message)
    {
        if (_connection.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection))
        {
            await Clients.Group(userRoomConnection.Room!)
                .SendAsync("ReceiveMessage", userRoomConnection.User, message, DateTime.Now);
        }
    }

    public override Task OnDisconnectedAsync(Exception? exp)
    {
        if (!_connection.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection))
        {
            return base.OnDisconnectedAsync(exp);
        }

        Clients.Group(userRoomConnection.Room!)
            .SendAsync("ReceiveMessage", "Chat manager", $"{userRoomConnection.User} left the chat");
        SendConnectedUser(userRoomConnection.Room!);
        return base.OnDisconnectedAsync(exp);
    }
    
    public Task SendConnectedUser(string room)
    {
        var users = _connection.Values
            .Where(u => u.Room == room)
            .Select(s => s.Room);

        return Clients.Group(room).SendAsync("ConnectedUser", users);
    }
}