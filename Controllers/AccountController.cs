using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalkStream_API.DTO;
using TalkStream_API.Entities;
using TalkStream_API.Repositories.UserRepository;
using TalkStream_API.Service;

namespace TalkStream_API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;
    
    public AccountController(IUserRepository userRepository, JwtService service)
    {
        _userRepository = userRepository;
        _jwtService = service;
    }
    [HttpPost("register")]
    public IActionResult Register(UserDto dto)
    {
        var user = new User
        {
            Uid = Guid.NewGuid().ToString(),
            Userame = dto.Username,
            Email = dto.Email,
            Role = Role.User,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _userRepository.RegisterUser(user);
        return Ok("User registered");
    }
    
    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
            var usr = _userRepository.GetUserByUsernameOrEmail(dto.UsernameOrEmail);
            
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, usr.Password))
            {
                return BadRequest("Wrong username or password");
            }
            var jwt = _jwtService.Generator(usr.Uid, usr.Role.ToString());
            Response.Cookies.Append("jwt", jwt, new CookieOptions {
                HttpOnly = true
            });
            return Ok(jwt);
    }
    
    [HttpGet("me")]
    [Authorize(Roles = "User")]
    public IActionResult GetUser()
    {
        var jwt = Request.Cookies["jwt"];
        // Parse the issuer from the JWT as an integer
        var token = _jwtService.Checker(jwt);
        var uid = token.Issuer;
        var user = _userRepository.GetUserById(uid);
        
        return Ok(user);
    }
}