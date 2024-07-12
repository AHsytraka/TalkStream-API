using System;
using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string SenderId { get; set; }
        
        public User Sender { get; set; }
        
        [Required]
        public string ReceiverId { get; set; }
        
        public User Receiver { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }
    }
}