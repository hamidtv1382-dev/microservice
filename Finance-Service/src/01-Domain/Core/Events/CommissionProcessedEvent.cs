namespace Finance_Service.src._01_Domain.Core.Events
{
    public class CommissionProcessedEvent : DomainEvent
    {
        public Guid CommissionId { get; }
        public Guid OrderId { get; }
        public Guid SellerId { get; }
        public decimal Amount { get; }

        public CommissionProcessedEvent(Guid commissionId, Guid orderId, Guid sellerId, decimal amount)
        {
            CommissionId = commissionId;
            OrderId = orderId;
            SellerId = sellerId;
            Amount = amount;
        }
    }
}
