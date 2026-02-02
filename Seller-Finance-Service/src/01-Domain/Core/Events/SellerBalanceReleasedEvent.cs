namespace Seller_Finance_Service.src._01_Domain.Core.Events
{
    public class SellerBalanceReleasedEvent : DomainEvent
    {
        public Guid SellerAccountId { get; }
        public Guid OrderId { get; }
        public decimal Amount { get; }

        public SellerBalanceReleasedEvent(Guid sellerAccountId, Guid orderId, decimal amount)
        {
            SellerAccountId = sellerAccountId;
            OrderId = orderId;
            Amount = amount;
        }
    }
}
