using TalkStream_API.DTO;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.FriendRequestRepository;

public interface IFriendshipRepository
{
    Task<IEnumerable<FriendDto>> GetAllFriendsAsync(string userId);
    Task<IEnumerable<Friendship>> GetAllReceivedFriendRequestsAsync(string userId);
    Task<Friendship> GetFriendshipAsync(string requesterId, string addresseeId);
    Task SendFriendRequestAsync(Friendship friendship);
    Task UpdateFriendshipAsync(Friendship friendship);
    Task DeleteFriendshipAsync(Friendship friendship);

}