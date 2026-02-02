using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Services.Interfaces;

namespace Order_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketApplicationService _basketService;
        private readonly ILogger<BasketsController> _logger;

        public BasketsController(IBasketApplicationService basketService, ILogger<BasketsController> logger)
        {
            _basketService = basketService;
            _logger = logger;
        }


        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("sub")?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            throw new UnauthorizedAccessException("Invalid User ID");
        }

        [HttpGet]
        public async Task<ActionResult<BasketDetailResponseDto>> GetBasket()
        {
            var userId = GetUserId();
            var result = await _basketService.GetBasketAsync(userId);
            return Ok(result);
        }

        [HttpPost("items")]
        public async Task<ActionResult<BasketDetailResponseDto>> AddItem([FromBody] AddItemToBasketRequestDto request)
        {
            var userId = GetUserId();
            var result = await _basketService.AddItemAsync(userId, request);
            return Ok(result);
        }

        [HttpPut("items")]
        public async Task<ActionResult<BasketDetailResponseDto>> UpdateItem([FromBody] UpdateBasketItemRequestDto request)
        {
            var userId = GetUserId();
            var result = await _basketService.UpdateItemAsync(userId, request);
            return Ok(result);
        }

        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<BasketDetailResponseDto>> RemoveItem(int productId)
        {
            var userId = GetUserId();
            var request = new RemoveItemFromBasketRequestDto { ProductId = productId };
            var result = await _basketService.RemoveItemAsync(userId, request);
            return Ok(result);
        }

        [HttpPost("apply-discount")]
        public async Task<ActionResult<BasketDetailResponseDto>> ApplyDiscount([FromBody] ApplyDiscountRequestDto request)
        {
            var userId = GetUserId();
            var result = await _basketService.ApplyDiscountAsync(userId, request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> ClearBasket()
        {
            var userId = GetUserId();
            await _basketService.ClearBasketAsync(userId);
            return NoContent();
        }
    }
}
