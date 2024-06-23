using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TalkStream_API.Entities;

public class User
{
    [Length(36,36)]
    [Key]
    public string Uid { get; set; }
    public string Userame { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}