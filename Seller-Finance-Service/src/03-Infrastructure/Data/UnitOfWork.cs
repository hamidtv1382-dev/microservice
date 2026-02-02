using Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Seller_Finance_Service.src._03_Infrastructure.Repositories;

namespace Seller_Finance_Service.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ISellerAccountRepository _sellerAccountRepository;
        private ISellerTransactionRepository _sellerTransactionRepository;
        private ISellerPayoutRepository _sellerPayoutRepository;
        private ISellerHoldRepository _sellerHoldRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ISellerAccountRepository SellerAccounts
        {
            get
            {
                if (_sellerAccountRepository == null)
                    _sellerAccountRepository = new SellerAccountRepository(_context);
                return _sellerAccountRepository;
            }
        }

        public ISellerTransactionRepository SellerTransactions
        {
            get
            {
                if (_sellerTransactionRepository == null)
                    _sellerTransactionRepository = new SellerTransactionRepository(_context);
                return _sellerTransactionRepository;
            }
        }

        public ISellerPayoutRepository SellerPayouts
        {
            get
            {
                if (_sellerPayoutRepository == null)
                    _sellerPayoutRepository = new SellerPayoutRepository(_context);
                return _sellerPayoutRepository;
            }
        }

        public ISellerHoldRepository SellerHolds
        {
            get
            {
                if (_sellerHoldRepository == null)
                    _sellerHoldRepository = new SellerHoldRepository(_context);
                return _sellerHoldRepository;
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
