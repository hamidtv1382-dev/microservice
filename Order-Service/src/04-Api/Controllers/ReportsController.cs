using Microsoft.AspNetCore.Mvc;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;

namespace Order_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IOrderRepository orderRepository, ILogger<ReportsController> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        // Admin endpoint to get filtered orders
        [HttpGet("orders")]
        public async Task<ActionResult<object>> GetOrders([FromQuery] GetOrdersFilterRequestDto filter)
        {
            // Note: Ideally, this should be in an IReportApplicationService
            var allOrders = (await _orderRepository.GetAllAsync()).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Status))
            {
                if (Enum.TryParse(filter.Status, true, out _01_Domain.Core.Enums.OrderStatus status))
                {
                    allOrders = allOrders.Where(o => o.Status == status);
                }
            }

            if (filter.StartDate.HasValue)
            {
                allOrders = allOrders.Where(o => o.CreatedAt >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                allOrders = allOrders.Where(o => o.CreatedAt <= filter.EndDate.Value);
            }

            var pagedOrders = allOrders
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var response = new
            {
                Total = allOrders.Count(),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Data = pagedOrders.Select(o => new OrderSummaryResponseDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber.Value,
                    Status = o.Status.ToString(),
                    FinalAmount = o.FinalAmount.Value,
                    CreatedAt = o.CreatedAt
                }).ToList()
            };

            return Ok(response);
        }

        // Endpoint for seller specific data
        [HttpGet("seller-orders")]
        public async Task<ActionResult<IEnumerable<SellerOrdersResponseDto>>> GetSellerOrders([FromQuery] GetSellerOrdersRequestDto request)
        {
            // Note: In a real scenario, we need to identify the seller (e.g., from Token or Route param)
            // and filter orders by products belonging to that seller.
            // Since Order contains OrderItems, and Items have ProductId, we join or filter.
            // For simplicity, returning Delivered orders as an example of 'Completed'.

            var allOrders = await _orderRepository.GetAllAsync();

            // Mock logic: Return completed orders as seller orders for now
            var completedOrders = allOrders
                .Where(o => o.Status == _01_Domain.Core.Enums.OrderStatus.Delivered)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(o => new SellerOrdersResponseDto
                {
                    OrderId = o.Id,
                    OrderNumber = o.OrderNumber.Value,
                    Status = o.Status.ToString(),
                    TotalAmount = o.TotalAmount.Value,
                    OrderDate = o.OrderDate ?? o.CreatedAt,
                    BuyerFullName = $"{o.ShippingAddress.FirstName} {o.ShippingAddress.LastName}",
                    ItemCount = o.Items.Count
                })
                .ToList();

            return Ok(completedOrders);
        }
    }
}
