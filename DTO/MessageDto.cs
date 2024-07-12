using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.DTO
{
    public class PrivateChatMessageDto
    {
        [Required]
        public string SenderId { get; set; }
        
        [Required]
        public string ReceiverId { get; set; }
        
        public string SenderUsername { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        
        public DateTime Timestamp { get; set; }
    }
}