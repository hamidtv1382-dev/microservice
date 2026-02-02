using Microsoft.EntityFrameworkCore;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._03_Infrastructure.Data;

namespace Order_Service.src._03_Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderNumber.Value == orderNumber);
        }

        public async Task<IEnumerable<Order>> GetByBuyerIdAsync(Guid buyerId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.BuyerId == buyerId && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => !o.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public void Delete(Order order)
        {
            order.MarkAsDeleted();
            _context.Orders.Update(order);
        }
    }
}
