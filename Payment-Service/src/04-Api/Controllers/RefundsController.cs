using Microsoft.AspNetCore.Mvc;
using Payment_Service.src._02_Application.DTOs.Requests;
using Payment_Service.src._02_Application.DTOs.Responses;
using Payment_Service.src._02_Application.Services.Interfaces;

namespace Payment_Service.src._04_Api.Controllers
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

        [HttpPost]
        public async Task<ActionResult<RefundResponseDto>> RequestRefund([FromBody] RequestRefundRequestDto request)
        {
            try
            {
                var result = await _refundService.RequestRefundAsync(request);
                return CreatedAtAction(nameof(GetRefundStatus), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting refund");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RefundResponseDto>> GetRefundStatus(Guid id)
        {
            try
            {
                var result = await _refundService.GetRefundStatusAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting refund status");
                return NotFound(ex.Message);
            }
        }
    }
}
