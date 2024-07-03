namespace TalkStream_API.DTO;

public record RegisterUserDto(string Username, string Email, string Password);
public record LoginDto(string UsernameOrEmail, string Password);
public record LoginResponseDto(string Uid,string Username, string Email ,string Jwt);