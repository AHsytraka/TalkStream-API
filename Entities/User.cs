using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TalkStream_API.Entities;

[Table("User")]
public class User : IdentityUser
{
    
}