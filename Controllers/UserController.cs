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
    
    [HttpPost("{requesterId}/sendFriendRequest/{addresseeId}")]
    public async Task<IActionResult> SendFriendRequest(string requesterId, string addresseeId)
    {
        await _userRepository.SendFriendRequestAsync(requesterId, addresseeId);
        await _hubContext.Clients.User(addresseeId).SendAsync("ReceiveFriendRequest", "Vous avez une demande d'ami");
        return Ok("Demande envoyer");
    }

    [HttpPost("respondToFriendRequest/{requestId}")]
    public async Task<IActionResult> RespondToFriendRequest(string requestId, bool isAccepted)
    {
        var response = await _userRepository.RespondToFriendRequestAsync(requestId, isAccepted);
        return Ok(response);
    }

    [HttpGet("sentFriendRequest")]
    public async Task<IActionResult> SentFriendRequest(string uid)
    {
        var request = await _userRepository.GetSentFriendRequestsAsync(uid);
        return Ok(request);
    }
    
    [HttpGet("ReceivedFriendRequest")]
    public async Task<IActionResult> ReceivedFriendRequest(string uid)
    {
        var request = await _userRepository.GetReceivedFriendRequestsAsync(uid);
        return Ok(request);
    }

    [HttpGet("Friends")]
    public async Task<IActionResult> GetFriends(string uid)
    {
        var friends = await _userRepository.GetUsersFriend(uid);
        return Ok(friends);
    }
}
