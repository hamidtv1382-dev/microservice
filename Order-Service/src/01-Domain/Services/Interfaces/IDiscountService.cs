using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<Discount?> ValidateDiscountAsync(string code, Money orderTotal);
        Money CalculateDiscountAmount(Discount discount, Money orderTotal);
    }
}
