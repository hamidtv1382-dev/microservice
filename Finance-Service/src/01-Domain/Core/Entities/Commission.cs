using Finance_Service.src._01_Domain.Core.Common;
using Finance_Service.src._01_Domain.Core.Enums;
using Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Finance_Service.src._01_Domain.Core.Entities
{
    public class Commission : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Guid SellerId { get; private set; }
        public Money Amount { get; private set; }
        public CommissionType Type { get; private set; }
        public Money RatePercentage { get; private set; }
        public bool IsSettled { get; private set; }
        public DateTime? SettledAt { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private Commission() { }

        public Commission(Guid orderId, Guid sellerId, Money amount, CommissionType type, decimal ratePercentage, string createdBy)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            SellerId = sellerId;
            Amount = amount;
            Type = type;
            RatePercentage = new Money(ratePercentage, "Percent");
            IsSettled = false;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void MarkAsSettled()
        {
            if (IsSettled) throw new InvalidOperationException("Commission is already settled.");
            IsSettled = true;
            SettledAt = DateTime.UtcNow;
            SetUpdatedAt();
        }
    }
}
