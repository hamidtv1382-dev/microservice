using Seller_Finance_Service.src._01_Domain.Core.Common;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._01_Domain.Core.Entities
{
    public class SellerBalance : ValueObject
    {
        public Money AvailableBalance { get; private set; }
        public Money PendingBalance { get; private set; } // Not released yet
        public Money HoldBalance { get; private set; } // Disputes or holds

        public SellerBalance()
        {
            AvailableBalance = new Money(0, "IRR");
            PendingBalance = new Money(0, "IRR");
            HoldBalance = new Money(0, "IRR");
        }

        public SellerBalance(decimal available, decimal pending, decimal hold, string currency)
        {
            AvailableBalance = new Money(available, currency);
            PendingBalance = new Money(pending, currency);
            HoldBalance = new Money(hold, currency);
        }

        public void AddToPending(Money amount)
        {
            PendingBalance = PendingBalance.Add(amount);
        }

        public void ReleaseFromPendingToAvailable(Money amount)
        {
            if (PendingBalance.Amount < amount.Amount)
                throw new InvalidOperationException("Insufficient pending balance to release.");

            PendingBalance = PendingBalance.Subtract(amount);
            AvailableBalance = AvailableBalance.Add(amount);
        }

        public void DeductFromAvailable(Money amount)
        {
            if (AvailableBalance.Amount < amount.Amount)
                throw new InvalidOperationException("Insufficient available balance.");

            AvailableBalance = AvailableBalance.Subtract(amount);
        }

        public void AddToHold(Money amount)
        {
            HoldBalance = HoldBalance.Add(amount);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AvailableBalance;
            yield return PendingBalance;
            yield return HoldBalance;
        }
    }
}
