using TalkStream_API.DTO;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.GroupRepository;

public interface IGroupRepository
{
    Task<Group> CreateGroupAsync(CreateGroupDto groupDto);
    Task<IEnumerable<GroupMessage>> GetGroupMessagesAsync(int groupId);
    Task<GroupMessage> SendMessageAsync(int groupId, SendMessageDto messageDto);
}