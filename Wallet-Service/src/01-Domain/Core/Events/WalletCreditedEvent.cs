namespace Wallet_Service.src._01_Domain.Core.Events
{
    public class WalletCreditedEvent : DomainEvent
    {
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string ReferenceId { get; }

        public WalletCreditedEvent(Guid walletId, decimal amount, string referenceId)
        {
            WalletId = walletId;
            Amount = amount;
            ReferenceId = referenceId;
        }
    }
}
