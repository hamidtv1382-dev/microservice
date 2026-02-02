using Messaging_Service.src._02_Application.DTOs.Requests;
using Messaging_Service.src._02_Application.DTOs.Responses;
using Messaging_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Messaging_Service.src._04_Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageApplicationService _messageService;

        public MessagesController(IMessageApplicationService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDto request)
        {
            var messageId = await _messageService.SendMessageAsync(request);
            return Ok(new { MessageId = messageId });
        }

        [HttpPost("{messageId}/approve")]
        public async Task<IActionResult> ApproveMessage(Guid messageId)
        {
            await _messageService.ApproveMessageAsync(messageId);
            return NoContent(); 
        }

        [HttpGet("chat/{chatId}")]
        public async Task<ActionResult<IEnumerable<MessageResponseDto>>> GetChatMessages(Guid chatId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var messages = await _messageService.GetChatMessagesAsync(chatId, page, pageSize);
            return Ok(messages);
        }
    }
}
