using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalkStream_API.Entities
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        public string CreatorId { get; set; }
        public User Creator { get; set; }
        
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }
    }

    public class GroupMessage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }
        
        [Required]
        public string SenderId { get; set; }
        public User Sender { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }
    }

    public class UserGroup
    {
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}