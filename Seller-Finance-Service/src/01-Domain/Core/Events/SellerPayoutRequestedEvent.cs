namespace Seller_Finance_Service.src._01_Domain.Core.Events
{
    public class SellerPayoutRequestedEvent : DomainEvent
    {
        public Guid SellerAccountId { get; }
        public Guid PayoutId { get; }
        public decimal Amount { get; }

        public SellerPayoutRequestedEvent(Guid sellerAccountId, Guid payoutId, decimal amount)
        {
            SellerAccountId = sellerAccountId;
            PayoutId = payoutId;
            Amount = amount;
        }
    }
}
