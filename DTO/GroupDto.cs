namespace TalkStream_API.DTO
{
    public class GroupCreateDto
    {
        public string Name { get; set; }
        public string CreatorId { get; set; }
    }

    public class UserGroupDto
    {
        public string UserId { get; set; }
    }

    public class GroupMessageDto
    {
        public string SenderId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}