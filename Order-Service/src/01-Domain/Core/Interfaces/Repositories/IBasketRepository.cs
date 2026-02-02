using Order_Service.src._01_Domain.Core.Entities;

namespace Order_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IBasketRepository
    {
        Task<Basket?> GetByIdAsync(Guid id);
        Task<Basket?> GetByBuyerIdAsync(Guid buyerId);
        Task<Basket?> GetByBuyerIdWithDiscountAsync(Guid buyerId);

        Task AddAsync(Basket basket);
        void Update(Basket basket);
        void Delete(Basket basket);
    }
}