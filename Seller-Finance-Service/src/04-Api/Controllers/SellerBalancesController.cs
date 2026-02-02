using Microsoft.AspNetCore.Mvc;
using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;

namespace Seller_Finance_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerBalancesController : ControllerBase
    {
        private readonly ISellerBalanceApplicationService _sellerBalanceApplicationService;

        public SellerBalancesController(ISellerBalanceApplicationService sellerBalanceApplicationService)
        {
            _sellerBalanceApplicationService = sellerBalanceApplicationService;
        }

        [HttpGet("{sellerId}")]
        public async Task<ActionResult<SellerBalanceResponseDto>> GetBalance(Guid sellerId)
        {
            var result = await _sellerBalanceApplicationService.GetBalanceAsync(sellerId);
            return Ok(result);
        }

        [HttpPost("earning")]
        public async Task<ActionResult<SellerTransactionResponseDto>> RecordEarning([FromBody] RecordSellerEarningRequestDto request)
        {
            var result = await _sellerBalanceApplicationService.RecordEarningAsync(request);
            return Ok(result);
        }

        [HttpPost("release")]
        public async Task<ActionResult<SellerTransactionResponseDto>> ReleaseBalance([FromBody] ReleaseSellerBalanceRequestDto request)
        {
            var result = await _sellerBalanceApplicationService.ReleaseBalanceAsync(request);
            return Ok(result);
        }

        [HttpPost("hold")]
        public async Task<IActionResult> HoldBalance(Guid sellerId, decimal amount, string reason, string description)
        {
            var result = await _sellerBalanceApplicationService.HoldBalanceAsync(sellerId, amount, reason, description);
            if (result) return Ok();
            return BadRequest("Failed to hold balance.");
        }
    }
}