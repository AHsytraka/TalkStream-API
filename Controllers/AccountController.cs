using Microsoft.AspNetCore.Mvc;
using TalkStream_API.Entities;
using TalkStream_API.Repositories.AccountRepository;

namespace TalkStream_API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController (IAccountRepository accountRepository) : ControllerBase
{
    private IAccountRepository _accountRepository = accountRepository;
    
    [HttpPost("/{Id}")]
    public IActionResult GetUserAccount([FromRoute] string Id)
    {
        var account = _accountRepository.GetUserAccount(Id);
        return Ok(account);
    }
}