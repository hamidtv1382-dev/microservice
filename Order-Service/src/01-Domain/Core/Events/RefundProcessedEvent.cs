namespace Order_Service.src._01_Domain.Core.Events
{
    public class RefundProcessedEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public Guid RefundId { get; }
        public decimal Amount { get; }
        public DateTime OccurredOn { get; }

        public RefundProcessedEvent(Guid orderId, Guid refundId, decimal amount)
        {
            OrderId = orderId;
            RefundId = refundId;
            Amount = amount;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
