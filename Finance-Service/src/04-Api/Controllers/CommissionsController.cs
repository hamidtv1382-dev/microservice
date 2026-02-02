using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;
using Finance_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Finance_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommissionsController : ControllerBase
    {
        private readonly IFinanceApplicationService _financeService;
        private readonly ILogger<CommissionsController> _logger;

        public CommissionsController(IFinanceApplicationService financeService, ILogger<CommissionsController> logger)
        {
            _financeService = financeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CommissionResponseDto>> ProcessCommission([FromBody] ProcessCommissionRequestDto request)
        {
            try
            {
                var result = await _financeService.ProcessCommissionAsync(request);
                return CreatedAtAction(nameof(GetCommissionById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing commission");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommissionResponseDto>> GetCommissionById(Guid id)
        {
            try
            {
                var result = await _financeService.GetCommissionByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting commission");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("seller/{sellerId}")]
        public async Task<ActionResult<IEnumerable<CommissionResponseDto>>> GetCommissionsBySellerId(Guid sellerId)
        {
            try
            {
                var result = await _financeService.GetCommissionsBySellerIdAsync(sellerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting commissions for seller");
                return BadRequest(ex.Message);
            }
        }
    }
}