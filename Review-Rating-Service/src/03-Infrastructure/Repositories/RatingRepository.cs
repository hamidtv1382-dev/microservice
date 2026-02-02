using Microsoft.EntityFrameworkCore;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;
using Review_Rating_Service.src._01_Domain.Core.Interfaces.Repositories;
using Review_Rating_Service.src._03_Infrastructure.Data;

namespace Review_Rating_Service.src._03_Infrastructure.Repositories
{

    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rating?> GetByIdAsync(Guid id)
        {
            return await _context.Reviews
                .SelectMany(r => r.Ratings)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rating>> GetByReviewIdAsync(Guid reviewId)
        {
            var review = await _context.Reviews
                .Include(r => r.Ratings)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
            {
                return Enumerable.Empty<Rating>();
            }

            return review.Ratings;
        }

        public async Task AddAsync(Rating rating)
        {
            await _context.AddAsync(rating);
        }

        public async Task UpdateAsync(Rating rating)
        {
            var existingRating = await _context.Reviews
                .SelectMany(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Id == rating.Id);

            if (existingRating != null)
            {
                _context.Entry(existingRating).CurrentValues.SetValues(rating);
            }
            else
            {
                _context.Update(rating);
            }
        }

        public async Task DeleteAsync(Rating rating)
        {
            var existingRating = await _context.Reviews
                .SelectMany(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Id == rating.Id);

            if (existingRating != null)
            {
                _context.Remove(existingRating);
            }
        }
    }
}
