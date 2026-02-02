using Finance_Service.src._01_Domain.Core.Common;
using Finance_Service.src._01_Domain.Core.Enums;
using Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Finance_Service.src._01_Domain.Core.Entities
{
    public class Fee : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Money Amount { get; private set; }
        public FeeType Type { get; private set; }
        public string Description { get; private set; }
        public Guid? SellerId { get; private set; }
        public bool IsPaid { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private Fee() { }

        public Fee(Guid orderId, Money amount, FeeType type, string description, Guid? sellerId, string createdBy)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Amount = amount;
            Type = type;
            Description = description;
            SellerId = sellerId;
            IsPaid = false;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void MarkAsPaid()
        {
            if (IsPaid) throw new InvalidOperationException("Fee is already paid.");
            IsPaid = true;
            PaidAt = DateTime.UtcNow;
            SetUpdatedAt();
        }
    }
}
