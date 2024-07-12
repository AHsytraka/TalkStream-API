using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.DTO;
using TalkStream_API.Entities;
using TalkStream_API.Hub;

namespace TalkStream_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<MessagingHub> _hubContext;
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context, IHubContext<MessagingHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
        {
            var sender = await _context.Users.FindAsync(messageDto.SenderId);
            var receiver = await _context.Users.FindAsync(messageDto.ReceiverId);

            if (sender == null || receiver == null)
            {
                return NotFound("Sender or Receiver not found");
            }

            var message = new Message
            {
                SenderId = messageDto.SenderId,
                SenderUsername = sender.Username,
                ReceiverId = messageDto.ReceiverId,
                ReceiverUsername = receiver.Username,
                Content = messageDto.Content,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(messageDto.ReceiverId).SendAsync("ReceiveMessage", messageDto.SenderId, sender.Username, messageDto.Content, message.Timestamp);
            await _hubContext.Clients.User(messageDto.SenderId).SendAsync("ReceiveMessage", messageDto.SenderId, sender.Username, messageDto.Content, message.Timestamp);

            return Ok();
        }

        [HttpGet("{currentUserId}/{otherUserId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(string currentUserId, string otherUserId)
        {
            var messages = await _context.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                            (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new MessageDto
                {
                    SenderId = m.SenderId,
                    SenderUsername = m.SenderUsername,
                    ReceiverId = m.ReceiverId,
                    ReceiverUsername = m.ReceiverUsername,
                    Content = m.Content,
                    Timestamp = m.Timestamp
                })
                .ToListAsync();

            return Ok(messages);
        }
    }
}
