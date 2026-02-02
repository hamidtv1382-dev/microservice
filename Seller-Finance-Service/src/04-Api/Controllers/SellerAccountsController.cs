using Microsoft.AspNetCore.Mvc;
using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;

namespace Seller_Finance_Service.src._04_Api.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class SellerAccountsController : ControllerBase
    {
        private readonly ISellerAccountApplicationService _accountAppService;

        public SellerAccountsController(ISellerAccountApplicationService accountAppService)
        {
            _accountAppService = accountAppService;
        }

        /// <summary>
        /// ایجاد حساب جدید برای فروشنده
        /// </summary>
        
        [HttpPost]
        public async Task<ActionResult<SellerAccountResponseDto>> CreateAccount([FromBody] CreateSellerAccountRequestDto request)
        {
            var result = await _accountAppService.CreateAccountAsync(request);
            return CreatedAtAction(nameof(GetAccount), new { sellerId = result.SellerId }, result);
        }

        /// <summary>
        /// دریافت اطلاعات حساب فروشنده
        /// </summary>
        [HttpGet("{sellerId}")]
        public async Task<ActionResult<SellerAccountResponseDto>> GetAccount(Guid sellerId)
        {
            var result = await _accountAppService.GetAccountBySellerIdAsync(sellerId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// به‌روزرسانی اطلاعات بانکی
        /// </summary>
        [HttpPut("bank-info")]
        public async Task<ActionResult<SellerAccountResponseDto>> UpdateBankInfo([FromBody] UpdateSellerBankAccountRequestDto request)
        {
            var result = await _accountAppService.UpdateBankAccountAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// فعال‌سازی حساب فروشنده
        /// </summary>
        [HttpPatch("{sellerId}/activate")]
        public async Task<IActionResult> ActivateAccount(Guid sellerId)
        {
            await _accountAppService.ActivateAccountAsync(sellerId);
            return NoContent();
        }

        /// <summary>
        /// غیرفعال‌سازی حساب فروشنده
        /// </summary>
        [HttpPatch("{sellerId}/deactivate")]
        public async Task<IActionResult> DeactivateAccount(Guid sellerId)
        {
            await _accountAppService.DeactivateAccountAsync(sellerId);
            return NoContent();
        }
    }
}
