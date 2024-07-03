namespace TalkStream_API.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}