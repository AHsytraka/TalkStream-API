namespace TalkStream_API.DTO;

// CreateGroupDto.cs
public class CreateGroupDto
{
    public string Name { get; set; }
    public string CreatorId { get; set; }
    public List<string> MemberIds { get; set; }
}

// SendMessageDto.cs
public class SendMessageDto
{
    public string SenderId { get; set; }
    public string Content { get; set; }
}