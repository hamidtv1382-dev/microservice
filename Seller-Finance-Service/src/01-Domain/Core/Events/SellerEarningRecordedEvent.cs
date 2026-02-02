namespace Seller_Finance_Service.src._01_Domain.Core.Events
{
    public class SellerEarningRecordedEvent : DomainEvent
    {
        public Guid SellerAccountId { get; }
        public Guid OrderId { get; }
        public decimal Amount { get; }

        public SellerEarningRecordedEvent(Guid sellerAccountId, Guid orderId, decimal amount)
        {
            SellerAccountId = sellerAccountId;
            OrderId = orderId;
            Amount = amount;
        }
    }
}
