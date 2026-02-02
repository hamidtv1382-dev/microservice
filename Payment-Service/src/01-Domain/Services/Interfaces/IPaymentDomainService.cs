using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._01_Domain.Core.ValueObjects;

namespace Payment_Service.src._01_Domain.Services.Interfaces
{
    public interface IPaymentDomainService
    {
        Task<Payment> InitiatePaymentAsync(Guid orderId, Money amount, string method);
        Task<bool> ValidatePaymentAsync(Payment payment);
        void CompletePayment(Payment payment, string externalTransactionId);
        void FailPayment(Payment payment, string reason);
    }
}
