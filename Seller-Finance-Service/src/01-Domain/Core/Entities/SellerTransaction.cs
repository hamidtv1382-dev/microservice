using Seller_Finance_Service.src._01_Domain.Core.Common;
using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._01_Domain.Core.Entities
{
    public class SellerTransaction : BaseEntity
    {
        public Guid SellerAccountId { get; private set; }
        public Money Amount { get; private set; }
        public SellerTransactionType Type { get; private set; }
        public SellerTransactionStatus Status { get; private set; }
        public string? ReferenceId { get; private set; } // OrderId or PayoutId
        public string Description { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private SellerTransaction() { }

        public SellerTransaction(Guid sellerAccountId, Money amount, SellerTransactionType type, string referenceId, string description, string createdBy)
        {
            Id = Guid.NewGuid();
            SellerAccountId = sellerAccountId;
            Amount = amount;
            Type = type;
            Status = SellerTransactionStatus.Success;
            ReferenceId = referenceId;
            Description = description;
            TransactionDate = DateTime.UtcNow;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void Fail(string reason)
        {
            Status = SellerTransactionStatus.Failed;
            Description = $"{Description} (Failed: {reason})";
            SetUpdatedAt();
        }
    }
}
