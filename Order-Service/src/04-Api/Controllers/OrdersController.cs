using Microsoft.AspNetCore.Mvc;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Services.Interfaces;
using System.Security.Claims;

namespace Order_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderApplicationService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderApplicationService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
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
        public async Task<ActionResult<OrderDetailResponseDto>> CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            var userId = GetUserId();
            var result = await _orderService.CreateOrderAsync(userId, request);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailResponseDto>> GetOrderById(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<OrderSummaryResponseDto>>> GetMyOrders()
        {
            var userId = GetUserId();
            var result = await _orderService.GetOrdersByBuyerIdAsync(userId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDetailResponseDto>> UpdateOrder(Guid id, [FromBody] UpdateOrderRequestDto request)
        {
            if (id != request.OrderId) return BadRequest("ID mismatch");

            var result = await _orderService.UpdateOrderAsync(request);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<OrderDetailResponseDto>> CancelOrder(Guid id, [FromBody] CancelOrderRequestDto request)
        {
            if (id != request.OrderId) return BadRequest("ID mismatch");

            request.OrderId = id;
            var result = await _orderService.CancelOrderAsync(request);
            return Ok(result);
        }

        [HttpPost("track")]
        public async Task<ActionResult<TrackOrderResponseDto>> TrackOrder([FromBody] TrackOrderRequestDto request)
        {
            var result = await _orderService.TrackOrderAsync(request);
            return Ok(result);
        }
    }
}
