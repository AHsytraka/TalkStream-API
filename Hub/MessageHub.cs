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
    
    // Create a group
        public async Task CreateGroup(string creatorId, string groupName)
        {
            var group = new Group
            {
                Name = groupName,
                CreatorId = creatorId,
                UserGroups = new List<UserGroup>
                {
                    new UserGroup { UserId = creatorId }
                }
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            await Clients.User(creatorId).SendAsync("GroupCreated", group.Id, groupName);
        }

        // Add a user to a group
        public async Task AddUserToGroup(int groupId, string userId, string currentUserId)
        {
            var group = _context.Groups.Include(g => g.Creator).FirstOrDefault(g => g.Id == groupId);
            if (group == null || group.CreatorId != currentUserId)
            {
                await Clients.Caller.SendAsync("Error", "You are not authorized to add users to this group.");
                return;
            }

            var userGroup = new UserGroup { UserId = userId, GroupId = groupId };
            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();

            await Clients.User(userId).SendAsync("AddedToGroup", groupId);
        }

        // Send a message to a group
        public async Task SendGroupMessage(string senderId, int groupId, string message)
        {
            var groupMessage = new GroupMessage
            {
                GroupId = groupId,
                SenderId = senderId,
                Content = message,
                Timestamp = DateTime.UtcNow
            };

            _context.GroupMessages.Add(groupMessage);
            await _context.SaveChangesAsync();

            var groupMembers = _context.UserGroups.Where(ug => ug.GroupId == groupId).Select(ug => ug.UserId).ToList();

            foreach (var memberId in groupMembers)
            {
                await Clients.User(memberId).SendAsync("ReceiveGroupMessage", groupId, senderId, message, groupMessage.Timestamp);
            }
        }

        // Quit a group
        public async Task QuitGroup(int groupId, string userId)
        {
            var userGroup = _context.UserGroups.FirstOrDefault(ug => ug.GroupId == groupId && ug.UserId == userId);
            if (userGroup == null)
            {
                await Clients.Caller.SendAsync("Error", "You are not part of this group.");
                return;
            }

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync();

            await Clients.User(userId).SendAsync("QuitGroup", groupId);
        }
}
