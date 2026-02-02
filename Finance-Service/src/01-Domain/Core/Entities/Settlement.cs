using Finance_Service.src._01_Domain.Core.Common;
using Finance_Service.src._01_Domain.Core.Enums;
using Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Finance_Service.src._01_Domain.Core.Entities
{
    public class Settlement : BaseEntity
    {
        public Guid SellerId { get; private set; }
        public Money TotalAmount { get; private set; }
        public SettlementStatus Status { get; private set; }
        public string? BankAccountInfo { get; private set; }
        public DateTime? SettledAt { get; private set; }
        public DateTime? DueDate { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private Settlement() { }

        public Settlement(Guid sellerId, Money totalAmount, DateTime dueDate, string createdBy)
        {
            Id = Guid.NewGuid();
            SellerId = sellerId;
            TotalAmount = totalAmount;
            Status = SettlementStatus.Pending;
            DueDate = dueDate;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void StartProcessing()
        {
            if (Status != SettlementStatus.Pending) throw new InvalidOperationException("Settlement is not in a valid state to start processing.");
            Status = SettlementStatus.Processing;
            SetUpdatedAt();
        }

        public void CompleteSettlement(string transactionRef)
        {
            if (Status != SettlementStatus.Processing) throw new InvalidOperationException("Settlement is not currently processing.");
            Status = SettlementStatus.Completed;
            SettledAt = DateTime.UtcNow;
            // In a real scenario, transaction reference would be stored or audited
            SetUpdatedAt();
        }

        public void FailSettlement(string reason)
        {
            if (Status != SettlementStatus.Processing) throw new InvalidOperationException("Settlement is not currently processing.");
            Status = SettlementStatus.Failed;
            SetUpdatedAt();
            // Log reason
        }
    }
}
