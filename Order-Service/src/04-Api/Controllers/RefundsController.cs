using Microsoft.AspNetCore.Mvc;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Services.Interfaces;
using System.Security.Claims;

namespace Order_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefundsController : ControllerBase
    {
        private readonly IRefundApplicationService _refundService;
        private readonly ILogger<RefundsController> _logger;

        public RefundsController(IRefundApplicationService refundService, ILogger<RefundsController> logger)
        {
            _refundService = refundService;
            _logger = logger;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("sub")?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            throw new UnauthorizedAccessException("Invalid User ID");
        }

        [HttpPost]
        public async Task<ActionResult<RefundResponseDto>> RequestRefund([FromBody] RequestRefundRequestDto request)
        {
            var userId = GetUserId();
            var result = await _refundService.RequestRefundAsync(userId, request);
            return Ok(result);
        }

        [HttpGet("{refundId}/status")]
        public async Task<ActionResult<RefundResponseDto>> GetRefundStatus(Guid refundId)
        {
            var result = await _refundService.GetRefundStatusAsync(refundId);
            return Ok(result);
        }
    }
}
