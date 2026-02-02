using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;
using Finance_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Finance_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeesController : ControllerBase
    {
        private readonly IFinanceApplicationService _financeService;
        private readonly ILogger<FeesController> _logger;

        public FeesController(IFinanceApplicationService financeService, ILogger<FeesController> logger)
        {
            _financeService = financeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<FeeResponseDto>> ApplyFee([FromBody] ApplyFeeRequestDto request)
        {
            try
            {
                var result = await _financeService.ApplyFeeAsync(request);
                return CreatedAtAction(nameof(GetFeeById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying fee");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeeResponseDto>> GetFeeById(Guid id)
        {
            try
            {
                var result = await _financeService.GetFeeByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting fee");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<FeeResponseDto>>> GetFeesByOrderId(Guid orderId)
        {
            try
            {
                var result = await _financeService.GetFeesByOrderIdAsync(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting fees for order");
                return BadRequest(ex.Message);
            }
        }
    }
}