using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;
using TalkStream_API.DTO;
using TalkStream_API.Entities;
using TalkStream_API.Hub;
using TalkStream_API.Repositories.GroupRepository;

namespace TalkStream_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IHubContext<GroupHub> _hubContext;
        private readonly AppDbContext _context;

        public GroupController(AppDbContext context, IHubContext<GroupHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreateDto groupCreateDto)
        {
            var creator = await _context.Users.FindAsync(groupCreateDto.CreatorId);
            if (creator == null)
            {
                return NotFound("Creator not found");
            }

            var group = new Group
            {
                Name = groupCreateDto.Name,
                CreatorId = groupCreateDto.CreatorId,
                Creator = creator
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return Ok(group);
        }

        [HttpPost("{groupId}/addUser")]
        public async Task<IActionResult> AddUserToGroup(int groupId, [FromBody] UserGroupDto userGroupDto)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(userGroupDto.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userGroup = new UserGroup
            {
                UserId = userGroupDto.UserId,
                GroupId = groupId,
                User = user,
                Group = group
            };

            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();
            return Ok($"{user.Username} has been added to the group");
        }

        [HttpPost("{groupId}/messages")]
        public async Task<IActionResult> SendMessage(int groupId, [FromBody] GroupMessageDto groupMessageDto)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }

            var sender = await _context.Users.FindAsync(groupMessageDto.SenderId);
            if (sender == null)
            {
                return NotFound("Sender not found");
            }

            var message = new GroupMessage
            {
                GroupId = groupId,
                SenderId = groupMessageDto.SenderId,
                Content = groupMessageDto.Content,
                Timestamp = DateTime.UtcNow,
                Sender = sender,
                Group = group
            };

            _context.GroupMessages.Add(message);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group(groupId.ToString()).SendAsync("ReceiveMessage", groupMessageDto.SenderId, sender.Username, groupMessageDto.Content, message.Timestamp);
            return Ok();
        }

        [HttpGet("{groupId}/messages")]
        public async Task<ActionResult<IEnumerable<GroupMessageDto>>> GetMessages(int groupId)
        {
            var messages = await _context.GroupMessages
                .Where(m => m.GroupId == groupId)
                .Select(m => new GroupMessageDto
                {
                    SenderId = m.SenderId,
                    Username = m.Sender.Username,
                    Content = m.Content,
                    Timestamp = m.Timestamp
                })
                .ToListAsync();

            return Ok(messages);
        }
    }
}
