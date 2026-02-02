using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;
using Seller_Finance_Service.src._01_Domain.Services.Interfaces;

namespace Seller_Finance_Service.src._01_Domain.Services.Implementations
{
    public class PayoutPolicyService : IPayoutPolicyService
    {
        public bool CanRequestPayout(SellerAccount account, Money requestedAmount)
        {
            if (!account.IsActive) return false;
            // Policy 1: Must have sufficient available balance
            if (account.Balance.AvailableBalance.Amount < requestedAmount.Amount) return false;

            // Policy 2: Minimum payout amount (e.g., 100,000 IRR)
            if (requestedAmount.Amount < 10000) return false;

            return true;
        }

        public SellerPayout CreatePayout(SellerAccount account, Money amount)
        {
            if (!CanRequestPayout(account, amount))
                throw new InvalidOperationException("Payout policy criteria not met.");

            return new SellerPayout(account.Id, amount, "Seller");
        }
    }
}
