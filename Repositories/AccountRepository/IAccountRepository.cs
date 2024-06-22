using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.AccountRepository;

public interface IAccountRepository
{
    public User GetUserAccount(string uid);
}