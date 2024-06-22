using TalkStream_API.Database;
using TalkStream_API.Entities;

namespace TalkStream_API.Repositories.AccountRepository;

public class AccountRepository : IAccountRepository
{
    private AppDbContext _appDbContext;
    public AccountRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public User GetUserAccount(string uid)
    {
        try
        {
            User user = _appDbContext.Users.Find(uid);
            if (user is null)
            {
                throw new ApplicationException("No result found");
            }
            return user;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error fetching user:", ex);
        }
    }
}