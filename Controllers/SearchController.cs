using Microsoft.AspNetCore.Mvc;
using TalkStream_API.Repositories.UserRepository;

namespace TalkStream_API.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public SearchController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpGet("{username}")]
    public IActionResult SearchUser(string username)
    {
        var users = _userRepository.GetUsersByUsername(username);
        return Ok(users);
    }

    [HttpGet("ByUsername")]
    public IActionResult GetUsersWithUsername(string username)
    {
        var users = _userRepository.GetUsersWithUsername(username);
        return Ok(users);
    }
}