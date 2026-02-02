using Messaging_Service.src._02_Application.DTOs.Requests;
using Messaging_Service.src._02_Application.DTOs.Responses;
using Messaging_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Messaging_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatApplicationService _chatService;

        public ChatsController(IChatApplicationService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequestDto request) // استفاده از FullName برای اطمینان
        {
            var chatId = await _chatService.CreateChatAsync(request);
            return Ok(new { ChatId = chatId });
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ChatSummaryResponseDto>>> GetUserChats(Guid userId)
        {
            var chats = await _chatService.GetUserChatsAsync(userId);
            return Ok(chats);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<ChatSummaryResponseDto>> GetChatById(Guid chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            return Ok(chat);
        }
    }
}
