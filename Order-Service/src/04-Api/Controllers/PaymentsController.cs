using Microsoft.AspNetCore.Mvc;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Services.Interfaces;
using System.Security.Claims;

namespace Order_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentApplicationService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentApplicationService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("sub")?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            throw new UnauthorizedAccessException("Invalid User ID");
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponseDto>> MakePayment([FromBody] MakePaymentRequestDto request)
        {
            var userId = GetUserId();
            var result = await _paymentService.MakePaymentAsync(userId, request);
            return Ok(result);
        }

        [HttpGet("{paymentId}/status")]
        public async Task<ActionResult<PaymentResponseDto>> GetPaymentStatus(Guid paymentId)
        {
            var result = await _paymentService.GetPaymentStatusAsync(paymentId);
            return Ok(result);
        }
    }
}
