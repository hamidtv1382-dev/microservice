using Microsoft.EntityFrameworkCore;
using Wallet_Service.src._01_Domain.Core.Entities;
using Wallet_Service.src._01_Domain.Core.Interfaces.Repositories;
using Wallet_Service.src._03_Infrastructure.Data;

namespace Wallet_Service.src._03_Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;

        public WalletRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet?> GetByIdAsync(Guid id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        }

        public async Task<Wallet?> GetByOwnerIdAsync(Guid ownerId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.OwnerId == ownerId && !w.IsDeleted);
        }

        public async Task AddAsync(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Wallet wallet)
        {
            wallet.MarkAsDeleted();
            _context.Wallets.Update(wallet);
            await Task.CompletedTask;
        }
    }
}
