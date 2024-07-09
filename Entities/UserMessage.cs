namespace TalkStream_API.Entities;

public class UserMessage
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string RecipientId { get; set; }
    public string Content { get; set; }
    public DateTime SentTime { get; set; }
}