namespace TalkStream_API.Repositories.FriendRepository;

public interface IFriendRepository
{
    public Task SendFriendRequestAsync(string senderId, string receiverId);
    
    public Task AcceptFriendRequestAsync(Guid requestId);
    
    public Task DeclineFriendRequestAsync(Guid requestId);
}