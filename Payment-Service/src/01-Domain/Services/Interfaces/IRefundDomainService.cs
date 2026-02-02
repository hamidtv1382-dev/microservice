using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._01_Domain.Core.ValueObjects;

namespace Payment_Service.src._01_Domain.Services.Interfaces
{
    public interface IRefundDomainService
    {
        Task<Refund> InitiateRefundAsync(Guid paymentId, Money amount, string reason);
        Task<bool> ValidateRefundAsync(Refund refund);
        void ProcessRefund(Refund refund, string externalRefundId);
    }
}
