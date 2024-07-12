using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.DTO;
using TalkStream_API.Entities;
using TalkStream_API.Hub;
using TalkStream_API.Helpers;

namespace TalkStream_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrivateChatController : ControllerBase
    {
        private readonly IHubContext<PrivateChatHub> _hubContext;
        private readonly AppDbContext _context;

        public PrivateChatController(AppDbContext context, IHubContext<PrivateChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost("{senderId}/{receiverId}/messages")]
        public async Task<IActionResult> SendMessage(string senderId, string receiverId, [FromBody] PrivateChatMessageDto privateChatMessageDto)
        {
            var sender = await _context.Users.FindAsync(senderId);
            if (sender == null)
            {
                return NotFound("Sender not found");
            }

            var receiver = await _context.Users.FindAsync(receiverId);
            if (receiver == null)
            {
                return NotFound("Receiver not found");
            }

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = privateChatMessageDto.Content,
                Timestamp = DateTime.UtcNow,
                Sender = sender,
                Receiver = receiver
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var groupName = GroupHelper.GetGroupName(senderId, receiverId);
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", senderId, sender.Username, privateChatMessageDto.Content, message.Timestamp);

            return Ok();
        }

        [HttpGet("{senderId}/{receiverId}/messages")]
        public async Task<ActionResult<IEnumerable<PrivateChatMessageDto>>> GetMessages(string senderId, string receiverId)
        {
            var messages = await _context.Messages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) || (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new PrivateChatMessageDto
                {
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    SenderUsername = m.Sender.Username,
                    Content = m.Content,
                    Timestamp = m.Timestamp
                })
                .ToListAsync();

            return Ok(messages);
        }
    }
}
