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
    public IActionResult Register(RegisterUserDto dto)
    {
        var user = new User
        {
            Uid = Guid.NewGuid().ToString(),
            Username = dto.Username,
            Email = dto.Email,
            Role = Role.User,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        var registered = _userRepository.RegisterUser(user);
        
        var jwt = _jwtService.Generator(registered.Uid, registered.Role.ToString()); 
        Response.Cookies.Append("jwt", jwt, new CookieOptions { 
            HttpOnly = true
        });

        var login = new LoginResponseDto(registered.Uid,registered.Username, registered.Email, jwt);
        if (!string.IsNullOrEmpty(login.Jwt))
        {
            return Ok(login);
        }

        return BadRequest("Veuillez verifier les informations saisie");
    }
    
    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var usr = _userRepository.GetUserByUsernameOrEmail(dto.UsernameOrEmail);
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, usr.Password)) 
        { 
            return BadRequest("Identifiant ou Mot de passe erroné");
        }
        var jwt = _jwtService.Generator(usr.Uid, usr.Role.ToString()); 
        Response.Cookies.Append("jwt", jwt, new CookieOptions { 
            HttpOnly = true
        });

        var user = new LoginResponseDto(usr.Uid,usr.Username, usr.Email, jwt);
        if (!string.IsNullOrEmpty(user.Jwt))
        {
            return Ok(user);
        }
        return BadRequest("Veuillez verifier les informations saisie");
    }
    
    [HttpGet("{uid}")] 
    public IActionResult GetUser(string uid)
    {
        // var jwt = Request.Cookies["jwt"];
        // Parse the issuer from the JWT as an integer
        // var token = _jwtService.Checker(jwt);
        // var uid = token.Issuer;
        var user = _userRepository.GetUserById(uid);
        
        return Ok(user);
    }
}