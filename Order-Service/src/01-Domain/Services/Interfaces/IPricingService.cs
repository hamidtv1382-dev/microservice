using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Services.Interfaces
{
    public interface IPricingService
    {
        Money CalculateItemTotal(Money unitPrice, int quantity);
        Money CalculateOrderTotal(IEnumerable<OrderItem> items);
        Money ApplyDiscount(Money totalAmount, Money discountValue);
    }
}
