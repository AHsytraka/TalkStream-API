using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TalkStream_API.Hub;
using TalkStream_API.Repositories.UserRepository;

namespace TalkStream_API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IHubContext<NotificationHub> _hubContext;

    public UsersController(IUserRepository userRepository, IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
        _userRepository = userRepository;
    }

    [HttpPost("{userId}/addFriend/{friendId}")]
    public async Task<IActionResult> AddFriend(string userId, string friendId)
    {
        if (userId == friendId)
        {
            return BadRequest("Users cannot add themselves as a friend.");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        var friend = await _userRepository.GetUserByIdAsync(friendId);

        if (user == null || friend == null)
        {
            return NotFound("User or friend not found.");
        }

        await _userRepository.AddFriendAsync(userId, friendId);

        return Ok($"Friend added successfully.");
    }
    
    [HttpPost("{requesterId}/sendFriendRequest/{addresseeId}")]
    public async Task<IActionResult> SendFriendRequest(string requesterId, string addresseeId)
    {
        await _userRepository.SendFriendRequestAsync(requesterId, addresseeId);
        await _hubContext.Clients.User(addresseeId).SendAsync("ReceiveFriendRequest", "Vous avez une demande d'ami");
        return Ok();
    }

    [HttpPost("respondToFriendRequest/{requestId}")]
    public async Task<IActionResult> RespondToFriendRequest(string requestId, bool isAccepted)
    {
        await _userRepository.RespondToFriendRequestAsync(requestId, isAccepted);
        return Ok();
    }
}