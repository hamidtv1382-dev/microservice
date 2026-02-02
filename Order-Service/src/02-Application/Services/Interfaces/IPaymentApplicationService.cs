using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;

namespace Order_Service.src._02_Application.Services.Interfaces
{
    public interface IPaymentApplicationService
    {
        Task<PaymentResponseDto> MakePaymentAsync(Guid buyerId, MakePaymentRequestDto request);
        Task<PaymentResponseDto> GetPaymentStatusAsync(Guid paymentId);
    }
}
