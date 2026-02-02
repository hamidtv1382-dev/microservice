using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.ValueObjects;
using Order_Service.src._01_Domain.Services.Interfaces;

namespace Order_Service.src._03_Infrastructure.Services.Internal
{
    public class PricingService : IPricingService
    {
        public Money CalculateItemTotal(Money unitPrice, int quantity)
        {
            return unitPrice * quantity;
        }

        public Money CalculateOrderTotal(IEnumerable<OrderItem> items)
        {
            if (items == null || !items.Any())
                return Money.Zero();

            return items.Aggregate(Money.Zero(), (acc, item) => acc + item.TotalPrice);
        }

        public Money ApplyDiscount(Money totalAmount, Money discountValue)
        {
            return totalAmount - discountValue;
        }
    }
}
