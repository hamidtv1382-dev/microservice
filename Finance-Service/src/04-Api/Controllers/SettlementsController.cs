using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;
using Finance_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Finance_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettlementsController : ControllerBase
    {
        private readonly ISettlementApplicationService _settlementService;
        private readonly ILogger<SettlementsController> _logger;

        public SettlementsController(ISettlementApplicationService settlementService, ILogger<SettlementsController> logger)
        {
            _settlementService = settlementService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<SettlementResponseDto>> CreateSettlement([FromBody] CreateSettlementRequestDto request)
        {
            try
            {
                var result = await _settlementService.CreateSettlementAsync(request);
                return CreatedAtAction(nameof(GetSettlementsBySellerId), new { sellerId = result.SellerId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating settlement");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/process")]
        public async Task<ActionResult<SettlementResponseDto>> ProcessSettlement(Guid id)
        {
            try
            {
                var result = await _settlementService.ProcessSettlementAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing settlement");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("seller/{sellerId}")]
        public async Task<ActionResult<IEnumerable<SettlementResponseDto>>> GetSettlementsBySellerId(Guid sellerId)
        {
            try
            {
                var result = await _settlementService.GetSettlementsBySellerIdAsync(sellerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting settlements for seller");
                return BadRequest(ex.Message);
            }
        }
    }
}
