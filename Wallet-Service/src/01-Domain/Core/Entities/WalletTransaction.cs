using Wallet_Service.src._01_Domain.Core.Common;
using Wallet_Service.src._01_Domain.Core.Enums;
using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._01_Domain.Core.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public Guid WalletId { get; private set; }
        public TransactionType Type { get; private set; }
        public Money Amount { get; private set; }
        public string ReferenceId { get; private set; }
        public string Description { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private WalletTransaction() { }

        public WalletTransaction(Guid walletId, TransactionType type, Money amount, string referenceId, string description, string createdBy)
        {
            Id = Guid.NewGuid();
            WalletId = walletId;
            Type = type;
            Amount = amount;
            ReferenceId = referenceId ?? Guid.NewGuid().ToString();
            Description = description;
            TransactionDate = DateTime.UtcNow;
            AuditInfo = new AuditInfo(createdBy);
        }
    }
}
