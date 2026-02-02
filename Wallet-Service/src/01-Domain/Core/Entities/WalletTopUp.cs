using Wallet_Service.src._01_Domain.Core.Common;
using Wallet_Service.src._01_Domain.Core.Enums;
using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._01_Domain.Core.Entities
{
    public class WalletTopUp : BaseEntity
    {
        public Guid WalletId { get; private set; }
        public Money Amount { get; private set; }
        public string? GatewayTransactionId { get; private set; }
        public TopUpStatus Status { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private WalletTopUp() { }

        public WalletTopUp(Guid walletId, Money amount, string createdBy)
        {
            Id = Guid.NewGuid();
            WalletId = walletId;
            Amount = amount;
            Status = TopUpStatus.Pending;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void MarkAsCompleted(string gatewayTransactionId)
        {
            if (Status != TopUpStatus.Pending) throw new InvalidOperationException("Top-up is not in a valid state.");
            Status = TopUpStatus.Success;
            GatewayTransactionId = gatewayTransactionId;
            CompletedAt = DateTime.UtcNow;
            SetUpdatedAt();
        }

        public void MarkAsFailed()
        {
            if (Status != TopUpStatus.Pending) throw new InvalidOperationException("Top-up is not in a valid state.");
            Status = TopUpStatus.Failed;
            SetUpdatedAt();
        }
    }

   
}
