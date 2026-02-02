using Microsoft.EntityFrameworkCore;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;
using Review_Rating_Service.src._01_Domain.Core.Interfaces.Repositories;
using Review_Rating_Service.src._03_Infrastructure.Data;

namespace Review_Rating_Service.src._03_Infrastructure.Repositories
{

    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _context.Reviews
                .Include(r => r.Ratings)
                .Include(r => r.Comments)
                .Include(r => r.Attachments)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        // تیپProductId از Guid به int تغییر یافت
        public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.Ratings)
                .Where(r => r.ProductId == productId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Reviews
                .Include(r => r.Ratings)
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<Review?> GetByUserAndProductAsync(Guid userId, int productId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId && !r.IsDeleted);
        }

        // ==========================================
        //  این متد مشکل‌ساز است و باید دقیقاً همین باشد:
        // ==========================================
        public async Task AddAsync(Review review)
        {
            // 1. افزودن Review اصلی
            await _context.Reviews.AddAsync(review);

            // 2. مدیریت دستی Ratings برای حل خطای FK
            // این حلقه به EF Core میگوید که این Ratingها جدید هستند و باید درج شوند
            foreach (var rating in review.Ratings)
            {
                var entry = _context.Entry(rating);

                // اگر Rating توسط Context دنبال نمی‌شود (Detached)، آن را اضافه کن
                if (entry.State == EntityState.Detached)
                {
                    // فقط اضافه کن اگر در لیست Local نباشد (جلوگیری از Duplicate)
                    if (!_context.Set<Rating>().Local.Any(r => r.Id == rating.Id))
                    {
                        _context.Set<Rating>().Add(rating);
                    }
                }
            }
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Review review)
        {
            review.MarkAsDeleted();
            _context.Reviews.Update(review);
            await Task.CompletedTask;
        }
    }
}
