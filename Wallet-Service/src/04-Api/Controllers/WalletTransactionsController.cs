using Microsoft.AspNetCore.Mvc;
using Wallet_Service.src._02_Application.DTOs.Requests;
using Wallet_Service.src._02_Application.DTOs.Responses;
using Wallet_Service.src._02_Application.Services.Interfaces;

namespace Wallet_Service.src._04_Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WalletTransactionsController : ControllerBase
    {
        private readonly IWalletTransactionApplicationService _transactionService;
        private readonly ILogger<WalletTransactionsController> _logger;

        public WalletTransactionsController(IWalletTransactionApplicationService transactionService, ILogger<WalletTransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletTransactionResponseDto>>> GetTransactions([FromQuery] GetWalletTransactionsRequestDto request)
        {
            try
            {
                var result = await _transactionService.GetTransactionsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wallet transactions");
                return BadRequest(ex.Message);
            }
        }
    }
}
