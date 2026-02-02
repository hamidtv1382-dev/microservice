using Microsoft.EntityFrameworkCore;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._03_Infrastructure.Data;

namespace Order_Service.src._03_Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly AppDbContext _context;

        public BasketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Basket?> GetByIdAsync(Guid id)
        {
            return await _context.Baskets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Basket?> GetByBuyerIdAsync(Guid buyerId)
        {
            return await _context.Baskets
                .Include(b => b.Items)
                .Include(b => b.AppliedDiscount)
                .FirstOrDefaultAsync(b => b.BuyerId == buyerId);
        }

        public async Task<Basket?> GetByBuyerIdWithDiscountAsync(Guid buyerId)
        {
            return await _context.Baskets
                .Include(b => b.Items)
                .Include(b => b.AppliedDiscount) 
                .FirstOrDefaultAsync(b => b.BuyerId == buyerId);
        }

        public async Task AddAsync(Basket basket)
        {
            await _context.Baskets.AddAsync(basket);
        }

        public void Update(Basket basket)
        {
            _context.Baskets.Update(basket);
        }

        public void Delete(Basket basket)
        {
            basket.MarkAsDeleted();
            _context.Baskets.Update(basket);
        }
    }
}
