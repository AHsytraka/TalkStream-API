using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.DTO;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.GroupRepository;

public class GroupRepository : IGroupRepository
{
    // private readonly AppDbContext _context;
    //
    // public GroupRepository(AppDbContext context)
    // {
    //     _context = context;
    // }
    //
    // public async Task<Group> CreateGroupAsync(CreateGroupDto groupDto)
    // {
    //     var group = new Group
    //     {
    //         Name = groupDto.Name,
    //         CreatorId = groupDto.CreatorId,
    //         UserGroups = groupDto.MemberIds.Select(id => new UserGroup { UserId = id }).ToList()
    //     };
    //
    //     _context.Groups.Add(group);
    //     await _context.SaveChangesAsync();
    //
    //     return group;
    // }
    //
    // public async Task<IEnumerable<GroupMessage>> GetGroupMessagesAsync(int groupId)
    // {
    //     return await _context.GroupMessages.Where(m => m.GroupId == groupId).ToListAsync();
    // }
    //
    // public async Task<GroupMessage> SendMessageAsync(int groupId, SendMessageDto messageDto)
    // {
    //     var message = new GroupMessage
    //     {
    //         GroupId = groupId,
    //         SenderId = messageDto.SenderId,
    //         Content = messageDto.Content,
    //         Timestamp = DateTime.UtcNow
    //     };
    //
    //     _context.GroupMessages.Add(message);
    //     await _context.SaveChangesAsync();
    //
    //     return message;
    // }

}