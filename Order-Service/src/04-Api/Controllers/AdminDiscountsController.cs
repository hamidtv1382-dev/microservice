using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Services.Interfaces;
using Order_Service.src._04_Api.Security;

namespace Order_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public class AdminDiscountsController : ControllerBase
    {
        private readonly IAdminDiscountApplicationService _adminDiscountService;

        public AdminDiscountsController(IAdminDiscountApplicationService adminDiscountService)
        {
            _adminDiscountService = adminDiscountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountSummaryResponseDto>>> GetAllDiscounts()
        {
            var discounts = await _adminDiscountService.GetAllDiscountsAsync();
            return Ok(discounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountDetailResponseDto>> GetDiscountById(Guid id)
        {
            var discount = await _adminDiscountService.GetDiscountByIdAsync(id);
            if (discount == null)
                return NotFound();

            return Ok(discount);
        }

        [HttpPost]
        public async Task<ActionResult<DiscountDetailResponseDto>> CreateDiscount([FromBody] CreateDiscountRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminDiscountService.CreateDiscountAsync(request);
            return CreatedAtAction(nameof(GetDiscountById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DiscountDetailResponseDto>> UpdateDiscount(Guid id, [FromBody] UpdateDiscountRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _adminDiscountService.UpdateDiscountAsync(id, request);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(Guid id)
        {
            try
            {
                await _adminDiscountService.DeleteDiscountAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
