using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalkStream_API.Entities;
using TalkStream_API.Repositories.UserRepository;

namespace TalkStream_API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    // private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    public AccountController(IUserRepository userRepository, UserManager<User> userManager)
    {
        _userManager = userManager;
        // _userRepository = userRepository;
    }
    
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetUser()
    {
        var user = HttpContext.User;
        
        if (user.Identity is not { IsAuthenticated: true }) return Unauthorized("Unauthorized to get this resource");

        var userId = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return Ok(_userManager.Users.First(u => u.Id == userId));

    }
}