using Wallet_Service.src._01_Domain.Core.Interfaces.Repositories;
using Wallet_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Wallet_Service.src._03_Infrastructure.Repositories;

namespace Wallet_Service.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IWalletRepository _walletRepository;
        private IWalletTransactionRepository _walletTransactionRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IWalletRepository Wallets
        {
            get
            {
                if (_walletRepository == null)
                    _walletRepository = new WalletRepository(_context);
                return _walletRepository;
            }
        }

        public IWalletTransactionRepository WalletTransactions
        {
            get
            {
                if (_walletTransactionRepository == null)
                    _walletTransactionRepository = new WalletTransactionRepository(_context);
                return _walletTransactionRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
