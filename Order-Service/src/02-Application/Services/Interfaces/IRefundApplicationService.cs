using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;

namespace Order_Service.src._02_Application.Services.Interfaces
{
    public interface IRefundApplicationService
    {
        Task<RefundResponseDto> RequestRefundAsync(Guid buyerId, RequestRefundRequestDto request);
        Task<RefundResponseDto> GetRefundStatusAsync(Guid refundId);
    }
}
