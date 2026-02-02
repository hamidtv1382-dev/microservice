using Microsoft.AspNetCore.Mvc;
using Payment_Service.src._02_Application.DTOs.Requests;
using Payment_Service.src._02_Application.DTOs.Responses;
using Payment_Service.src._02_Application.Services.Interfaces;

namespace Payment_Service.src._04_Api.Controllers
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

        [HttpPost]
        public async Task<ActionResult<PaymentResponseDto>> MakePayment([FromBody] MakePaymentRequestDto request)
        {
            try
            {
                var result = await _paymentService.MakePaymentAsync(request);
                return CreatedAtAction(nameof(GetPaymentStatus), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error making payment");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponseDto>> GetPaymentStatus(Guid id)
        {
            try
            {
                var result = await _paymentService.GetPaymentStatusAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment status");
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}/verify")]
        public async Task<ActionResult<PaymentResponseDto>> VerifyPayment(Guid id)
        {
            try
            {
                var result = await _paymentService.VerifyPaymentAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying payment");
                return BadRequest(ex.Message);
            }
        }
    }
}
