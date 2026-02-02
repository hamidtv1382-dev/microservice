using Finance_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IFeeRepository Fees { get; }
        ICommissionRepository Commissions { get; }
        ISettlementRepository Settlements { get; }

        Task<int> SaveChangesAsync();
    }
}
