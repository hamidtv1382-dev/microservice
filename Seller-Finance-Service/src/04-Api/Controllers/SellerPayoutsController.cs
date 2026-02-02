using Microsoft.AspNetCore.Mvc;
using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;

namespace Seller_Finance_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerPayoutsController : ControllerBase
    {
        private readonly ISellerPayoutApplicationService _sellerPayoutApplicationService;

        public SellerPayoutsController(ISellerPayoutApplicationService sellerPayoutApplicationService)
        {
            _sellerPayoutApplicationService = sellerPayoutApplicationService;
        }

        [HttpPost]
        public async Task<ActionResult<SellerPayoutResponseDto>> RequestPayout([FromBody] RequestSellerPayoutDto request)
        {
            var result = await _sellerPayoutApplicationService.RequestPayoutAsync(request);
            return Ok(result);
        }

        [HttpGet("{payoutId}")]
        public async Task<ActionResult<SellerPayoutResponseDto>> GetPayoutStatus(Guid payoutId)
        {
            var result = await _sellerPayoutApplicationService.GetPayoutStatusAsync(payoutId);
            return Ok(result);
        }
    }
}
