namespace Finance_Service.src._01_Domain.Core.Events
{
    public class FeeAppliedEvent : DomainEvent
    {
        public Guid FeeId { get; }
        public Guid OrderId { get; }
        public decimal Amount { get; }

        public FeeAppliedEvent(Guid feeId, Guid orderId, decimal amount)
        {
            FeeId = feeId;
            OrderId = orderId;
            Amount = amount;
        }
    }
}
