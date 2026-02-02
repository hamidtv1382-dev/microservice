using Microsoft.EntityFrameworkCore;
using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Seller_Finance_Service.src._03_Infrastructure.Data;

namespace Seller_Finance_Service.src._03_Infrastructure.Repositories
{
    public class SellerTransactionRepository : ISellerTransactionRepository
    {
        private readonly AppDbContext _context;

        public SellerTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SellerTransaction?> GetByIdAsync(Guid id)
        {
            return await _context.SellerTransactions.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<IEnumerable<SellerTransaction>> GetBySellerAccountIdAsync(Guid accountId)
        {
            return await _context.SellerTransactions
                .Where(t => t.SellerAccountId == accountId && !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SellerTransaction>> GetByReferenceIdAsync(string referenceId)
        {
            return await _context.SellerTransactions
                .Where(t => t.ReferenceId == referenceId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(SellerTransaction transaction)
        {
            await _context.SellerTransactions.AddAsync(transaction);
        }

        public async Task UpdateAsync(SellerTransaction transaction)
        {
            _context.SellerTransactions.Update(transaction);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(SellerTransaction transaction)
        {
            transaction.MarkAsDeleted();
            _context.SellerTransactions.Update(transaction);
            await Task.CompletedTask;
        }
    }
}
