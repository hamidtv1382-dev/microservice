using Payment_Service.src._02_Application.DTOs.Requests;
using Payment_Service.src._02_Application.DTOs.Responses;

namespace Payment_Service.src._02_Application.Services.Interfaces
{
    public interface IRefundApplicationService
    {
        Task<RefundResponseDto> RequestRefundAsync(RequestRefundRequestDto request);
        Task<RefundResponseDto> GetRefundStatusAsync(Guid refundId);
    }
}
