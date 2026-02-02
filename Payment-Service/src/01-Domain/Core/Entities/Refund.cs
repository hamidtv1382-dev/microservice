using Payment_Service.src._01_Domain.Core.Common;
using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.ValueObjects;

namespace Payment_Service.src._01_Domain.Core.Entities
{
    public class Refund : BaseEntity
    {
        public Guid PaymentId { get; private set; }
        public Money Amount { get; private set; }
        public RefundStatus Status { get; private set; }
        public string Reason { get; private set; }
        public string? ExternalRefundId { get; private set; }
        public DateTime? RefundedAt { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private Refund() { }

        public Refund(Guid paymentId, Money amount, string reason, string? requestedBy)
        {
            Id = Guid.NewGuid();
            PaymentId = paymentId;
            Amount = amount;
            Reason = reason;
            Status = RefundStatus.Pending;
            AuditInfo = new AuditInfo(requestedBy);
        }

        public void MarkAsProcessed(string externalRefundId)
        {
            if (Status != RefundStatus.Pending)
                throw new InvalidOperationException("Refund is not in a valid state to be processed.");

            Status = RefundStatus.Succeeded;
            ExternalRefundId = externalRefundId;
            RefundedAt = DateTime.UtcNow;
            SetUpdatedAt();
        }

        public void MarkAsFailed(string failureReason)
        {
            if (Status != RefundStatus.Pending)
                throw new InvalidOperationException("Refund is not in a valid state to be failed.");

            Status = RefundStatus.Failed;
            Reason = $"{Reason} | Failed: {failureReason}";
            SetUpdatedAt();
        }
    }
}
