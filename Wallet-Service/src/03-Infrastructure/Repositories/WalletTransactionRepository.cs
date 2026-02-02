using Microsoft.EntityFrameworkCore;
using Wallet_Service.src._01_Domain.Core.Entities;
using Wallet_Service.src._01_Domain.Core.Interfaces.Repositories;
using Wallet_Service.src._03_Infrastructure.Data;

namespace Wallet_Service.src._03_Infrastructure.Repositories
{
    public class WalletTransactionRepository : IWalletTransactionRepository
    {
        private readonly AppDbContext _context;

        public WalletTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WalletTransaction?> GetByIdAsync(Guid id)
        {
            return await _context.WalletTransactions.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(Guid walletId)
        {
            return await _context.WalletTransactions
                .Where(t => t.WalletId == walletId && !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task AddAsync(WalletTransaction transaction)
        {
            await _context.WalletTransactions.AddAsync(transaction);
        }

        public async Task UpdateAsync(WalletTransaction transaction)
        {
            _context.WalletTransactions.Update(transaction);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(WalletTransaction transaction)
        {
            transaction.MarkAsDeleted();
            _context.WalletTransactions.Update(transaction);
            await Task.CompletedTask;
        }
    }
}
