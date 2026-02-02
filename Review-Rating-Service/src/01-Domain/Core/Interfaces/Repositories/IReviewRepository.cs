using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;

namespace Review_Rating_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
        Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId);
        Task<Review?> GetByUserAndProductAsync(Guid userId, int productId);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
    }
}
