using Seller_Finance_Service.src._01_Domain.Core.Common;
using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._01_Domain.Core.Entities
{
    public class SellerHold : BaseEntity
    {
        public Guid SellerAccountId { get; private set; }
        public Money Amount { get; private set; }
        public HoldReasonType Reason { get; private set; }
        public string Description { get; private set; }
        public bool IsReleased { get; private set; }
        public DateTime? ReleasedAt { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private SellerHold() { }

        public SellerHold(Guid sellerAccountId, Money amount, HoldReasonType reason, string description, string createdBy)
        {
            Id = Guid.NewGuid();
            SellerAccountId = sellerAccountId;
            Amount = amount;
            Reason = reason;
            Description = description;
            IsReleased = false;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void Release()
        {
            if (IsReleased) throw new InvalidOperationException("Hold is already released.");
            IsReleased = true;
            ReleasedAt = DateTime.UtcNow;
            SetUpdatedAt();
        }
    }
}
