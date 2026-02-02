using Payment_Service.src._02_Application.DTOs.Requests;
using Payment_Service.src._02_Application.DTOs.Responses;

namespace Payment_Service.src._02_Application.Services.Interfaces
{
    public interface IPaymentApplicationService
    {
        Task<PaymentResponseDto> MakePaymentAsync(MakePaymentRequestDto request);
        Task<PaymentResponseDto> GetPaymentStatusAsync(Guid paymentId);
        Task<PaymentResponseDto> VerifyPaymentAsync(Guid paymentId);
    }
}
