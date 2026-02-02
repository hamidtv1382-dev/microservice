using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._01_Domain.Core.ValueObjects;
using Order_Service.src._01_Domain.Services.Interfaces;

namespace Order_Service.src._01_Domain.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<Discount?> ValidateDiscountAsync(string code, Money orderTotal)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var discount = await _discountRepository.GetByCodeAsync(code);

            if (discount == null)
                return null;

            if (!discount.IsActive)
                return null;

            if (!discount.CanApply(orderTotal))
                return null;

            return discount;
        }

        public Money CalculateDiscountAmount(Discount discount, Money orderTotal)
        {
            return discount.CalculateDiscountAmount(orderTotal);
        }
    }
}
