namespace Order_Service.src._01_Domain.Core.Events
{
    public class PaymentCompletedEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public Guid PaymentId { get; }
        public decimal Amount { get; }
        public DateTime OccurredOn { get; }

        public PaymentCompletedEvent(Guid orderId, Guid paymentId, decimal amount)
        {
            OrderId = orderId;
            PaymentId = paymentId;
            Amount = amount;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
