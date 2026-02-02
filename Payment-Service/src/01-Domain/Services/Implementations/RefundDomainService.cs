using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.ValueObjects;
using Payment_Service.src._01_Domain.Services.Interfaces;

namespace Payment_Service.src._01_Domain.Services.Implementations
{

    public class RefundDomainService : IRefundDomainService
    {
        public async Task<Refund> InitiateRefundAsync(Guid paymentId, Money amount, string reason)
        {
            if (paymentId == Guid.Empty) throw new ArgumentException("Invalid Payment ID");
            if (amount.Amount <= 0) throw new ArgumentException("Amount must be greater than zero");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Reason is required");

            var refund = new Refund(paymentId, amount, reason, "System");
            return await Task.FromResult(refund);
        }

        public Task<bool> ValidateRefundAsync(Refund refund)
        {
            if (refund == null) return Task.FromResult(false);
            if (refund.Amount.Amount <= 0) return Task.FromResult(false);
            if (refund.Status != RefundStatus.Pending)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }

        public void ProcessRefund(Refund refund, string externalRefundId)
        {
            if (refund == null) throw new ArgumentNullException(nameof(refund));
            refund.MarkAsProcessed(externalRefundId);
        }
    }
}
