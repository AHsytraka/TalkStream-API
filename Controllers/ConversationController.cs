using Microsoft.AspNetCore.Mvc;
using TalkStream_API.Repositories.MessageRepository;

namespace TalkStream_API.Controllers;

[ApiController]
[Route("[controller]")]
public class ConversationController:ControllerBase
{
    private readonly IMessageRepository _messageRepository;

    public ConversationController(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    [HttpGet("conversation")]
    public async Task<IActionResult> GetConversation(string senderId, string receiverId)
    {
        var messages = await _messageRepository.GetConversationAsync(senderId, receiverId);
        return Ok(messages);
    }
    
    [HttpGet("{uid}")]
    public async Task<IActionResult> GetUsersConversation(string uid)
    {
        var conversations = await _messageRepository.GetUserConversationsAsync(uid);
        return Ok(conversations);
    }
}