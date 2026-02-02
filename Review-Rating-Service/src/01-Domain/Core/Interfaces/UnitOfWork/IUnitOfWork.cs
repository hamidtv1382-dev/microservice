using Review_Rating_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Review_Rating_Service.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IReviewRepository Reviews { get; }
        IRatingRepository Ratings { get; }
        Task<int> SaveChangesAsync();
    }
}
