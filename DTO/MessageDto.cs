using System;

namespace TalkStream_API.DTO
{
    public class MessageDto
    {
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverUsername { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}