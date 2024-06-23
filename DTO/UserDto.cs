namespace TalkStream_API.DTO;

public record UserDto(string Username, string Email, string Password);
public record LoginDto(string UsernameOrEmail, string Password);