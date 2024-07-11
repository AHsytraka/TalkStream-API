using TalkStream_API.Entities;

namespace TalkStream_API.DTO;

public class ConversationDto
{
    public string OtherUserId { get; set; }
    public string OtherUserName { get; set; }
    public IEnumerable<Message> Messages { get; set; }
}