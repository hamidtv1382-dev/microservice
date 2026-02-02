using Microsoft.AspNetCore.Mvc;
using Wallet_Service.src._02_Application.DTOs.Requests;
using Wallet_Service.src._02_Application.DTOs.Responses;
using Wallet_Service.src._02_Application.Services.Interfaces;

namespace Wallet_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletApplicationService _walletService;
        private readonly ILogger<WalletsController> _logger;

        public WalletsController(IWalletApplicationService walletService, ILogger<WalletsController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        [HttpGet("balance/{ownerId}")]
        public async Task<ActionResult<WalletBalanceResponseDto>> GetBalance(Guid ownerId)
        {
            try
            {
                var result = await _walletService.GetBalanceAsync(ownerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wallet balance");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-funds")]
        public async Task<ActionResult<WalletTransactionResponseDto>> AddFunds([FromBody] AddFundsRequestDto request)
        {
            try
            {
                var result = await _walletService.AddFundsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding funds");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deduct-funds")]
        public async Task<ActionResult<WalletTransactionResponseDto>> DeductFunds([FromBody] DeductFundsRequestDto request)
        {
            try
            {
                var result = await _walletService.DeductFundsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deducting funds");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{userId}/deduct")]
        public async Task<ActionResult<WalletTransactionResponseDto>> DeductFundsByUserId(Guid userId, [FromBody] DeductFundsByUserIdRequestDto request)
        {
            try
            {
                // برای اطمینان از اینکه UserId در URL با UserId در Body یکی است
                if (userId != request.UserId)
                {
                    return BadRequest("UserId mismatch.");
                }

                var result = await _walletService.DeductFundsByUserIdAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deducting funds by UserId");
                return BadRequest(ex.Message);
            }
        }
    }
}