using Microsoft.AspNetCore.Mvc;
using TalkStream_API.DTO;
using TalkStream_API.Repositories.GroupRepository;

namespace TalkStream_API.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController : ControllerBase
{

        private readonly IGroupRepository _groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto groupDto)
        {
            try
            {
                var group = await _groupRepository.CreateGroupAsync(groupDto);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating group: {ex.Message}");
            }
        }
        
        [HttpGet("{groupId}/messages")]
        public async Task<IActionResult> GetGroupMessages(int groupId)
        {
            try
            {
                var messages = await _groupRepository.GetGroupMessagesAsync(groupId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching group messages: {ex.Message}");
            }
        }

        [HttpPost("{groupId}/messages")]
        public async Task<IActionResult> SendMessage(int groupId, [FromBody] SendMessageDto messageDto)
        {
            try
            {
                var message = await _groupRepository.SendMessageAsync(groupId, messageDto);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error sending message: {ex.Message}");
            }
        }
        
}