using Microsoft.AspNetCore.Mvc;
using TalkStream_API.Entities;
using TalkStream_API.Repositories.FriendRequestRepository;
using TalkStream_API.Repositories.UserRepository;

namespace TalkStream_API.Controllers;

[ApiController]
[Route("[controller]")]
public class FriendController : ControllerBase
{
    // private readonly IUserRepository _userRepository;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserRepository _userRepository;

    public FriendController(IFriendshipRepository friendshipRepository, IUserRepository userRepository)
    {
        _friendshipRepository = friendshipRepository;
        _userRepository = userRepository;
    }
    
    [HttpGet("friends/{userId}")]
    public async Task<ActionResult<IEnumerable<Friendship>>> GetFriends(string userId)
    {
        var friends = await _friendshipRepository.GetAllFriendsAsync(userId);

        if (friends == null || !friends.Any())
        {
            return Ok(new List<Friendship>());
        }

        return Ok(friends);
    }
    
    [HttpGet("received-requests/{userId}")]
    public async Task<ActionResult<IEnumerable<Friendship>>> GetReceivedFriendRequests(string userId)
    {
        var receivedRequests = await _friendshipRepository.GetAllReceivedFriendRequestsAsync(userId);

        if (receivedRequests == null || !receivedRequests.Any())
        {
            // Return an empty list with a 200 OK status
            return Ok(new List<Friendship>());
        }

        return Ok(receivedRequests);
    }
    
    [HttpPost("send")]
    public async Task<IActionResult> SendFriendRequest(string requesterId, string addresseeId)
    {
        var requester = await _userRepository.GetUserByIdAsync(requesterId);
        var addressee = await _userRepository.GetUserByIdAsync(addresseeId);

        if (requester == null || addressee == null)
        {
            return NotFound("One or both users not found.");
        }

        var existingFriendship = await _friendshipRepository.GetFriendshipAsync(requesterId, addresseeId);
        if (existingFriendship != null)
        {
            return BadRequest("Friend request already exists.");
        }

        var friendship = new Friendship
        {
            RequesterId = requesterId,
            AddresseeId = addresseeId,
            IsAccepted = false
        };

        await _friendshipRepository.SendFriendRequestAsync(friendship);
        return Ok("Friend request sent successfully.");
    }

    [HttpPost("respond")]
    public async Task<IActionResult> RespondToFriendRequest(string requesterId, string addresseeId, bool accept)
    {
        var friendship = await _friendshipRepository.GetFriendshipAsync(requesterId, addresseeId);
        if (friendship == null)
        {
            return NotFound("Friend request not found.");
        }

        if (accept)
        {
            friendship.IsAccepted = true;
        }
        else
        {
            // Assuming you want to delete the request if declined
            _friendshipRepository.DeleteFriendshipAsync(friendship);
        }

        await _friendshipRepository.UpdateFriendshipAsync(friendship);
        return Ok(accept ? "Friend request accepted." : "Friend request declined.");
    }
}
