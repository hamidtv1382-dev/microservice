using Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Finance_Service.src._03_Infrastructure.Repositories;

namespace Finance_Service.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IFeeRepository _feeRepository;
        private ICommissionRepository _commissionRepository;
        private ISettlementRepository _settlementRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IFeeRepository Fees
        {
            get
            {
                if (_feeRepository == null)
                    _feeRepository = new FeeRepository(_context);
                return _feeRepository;
            }
        }

        public ICommissionRepository Commissions
        {
            get
            {
                if (_commissionRepository == null)
                    _commissionRepository = new CommissionRepository(_context);
                return _commissionRepository;
            }
        }

        public ISettlementRepository Settlements
        {
            get
            {
                if (_settlementRepository == null)
                    _settlementRepository = new SettlementRepository(_context);
                return _settlementRepository;
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
