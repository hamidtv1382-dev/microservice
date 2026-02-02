using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;

namespace Review_Rating_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IRatingRepository
    {
        Task<Rating?> GetByIdAsync(Guid id);
        Task<IEnumerable<Rating>> GetByReviewIdAsync(Guid reviewId);
        Task AddAsync(Rating rating);
        Task UpdateAsync(Rating rating);
        Task DeleteAsync(Rating rating);
    }
}
