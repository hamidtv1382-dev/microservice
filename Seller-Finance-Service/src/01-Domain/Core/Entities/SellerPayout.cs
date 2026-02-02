using Seller_Finance_Service.src._01_Domain.Core.Common;
using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._01_Domain.Core.Entities
{
    public class SellerPayout : BaseEntity
    {
        public Guid SellerAccountId { get; private set; }
        public Money Amount { get; private set; }
        public PayoutStatus Status { get; private set; }
        public string? GatewayReferenceId { get; private set; }
        public DateTime? RequestedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string FailureReason { get; private set; } = string.Empty;
        public AuditInfo AuditInfo { get; private set; }

        private SellerPayout() { }

        public SellerPayout(Guid sellerAccountId, Money amount, string createdBy)
        {
            Id = Guid.NewGuid();
            SellerAccountId = sellerAccountId;
            Amount = amount;
            Status = PayoutStatus.Requested;
            RequestedAt = DateTime.UtcNow;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void MarkAsProcessing()
        {
            if (Status != PayoutStatus.Requested) throw new InvalidOperationException("Payout is not in Requested state.");
            Status = PayoutStatus.Processing;
            SetUpdatedAt();
        }

        public void MarkAsCompleted(string gatewayRef)
        {
            if (Status != PayoutStatus.Processing) throw new InvalidOperationException("Payout is not in Processing state.");
            Status = PayoutStatus.Completed;
            GatewayReferenceId = gatewayRef;
            ProcessedAt = DateTime.UtcNow;
            SetUpdatedAt();
        }

        public void MarkAsFailed(string reason)
        {
            if (Status != PayoutStatus.Processing) throw new InvalidOperationException("Payout is not in Processing state.");
            Status = PayoutStatus.Failed;
            FailureReason = reason;
            SetUpdatedAt();
        }
    }
}
