using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;

namespace Order_Service.src._02_Application.Services.Interfaces
{
    public interface IOrderApplicationService
    {
        Task<OrderDetailResponseDto> CreateOrderAsync(Guid buyerId, CreateOrderRequestDto request);
        Task<OrderDetailResponseDto> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderSummaryResponseDto>> GetOrdersByBuyerIdAsync(Guid buyerId);
        Task<OrderDetailResponseDto> UpdateOrderAsync(UpdateOrderRequestDto request);
        Task<OrderDetailResponseDto> CancelOrderAsync(CancelOrderRequestDto request);
        Task<TrackOrderResponseDto> TrackOrderAsync(TrackOrderRequestDto request);
    }
}
