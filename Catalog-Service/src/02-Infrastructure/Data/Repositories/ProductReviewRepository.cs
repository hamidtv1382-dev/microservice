using Catalog_Service.src._01_Domain.Core.Contracts.Repositories;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Catalog_Service.src._02_Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog_Service.src._02_Infrastructure.Data.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductReviewRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductReview> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Include(pr => pr.Product)
                .FirstOrDefaultAsync(pr => pr.Id == id && !pr.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => !pr.IsDeleted)
                .Include(pr => pr.Product)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && !pr.IsDeleted)
                .Include(pr => pr.Product)
                .OrderByDescending(pr => pr.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.UserId == userId && !pr.IsDeleted)
                .Include(pr => pr.Product)
                .OrderByDescending(pr => pr.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductReview> AddAsync(ProductReview review, CancellationToken cancellationToken = default)
        {
            await _dbContext.ProductReviews.AddAsync(review, cancellationToken);
            return review;
        }

        public void Update(ProductReview review)
        {
            _dbContext.ProductReviews.Update(review);
        }

        public void Remove(ProductReview review)
        {
            // Soft Delete Implementation
            var entry = _dbContext.Entry(review);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.ProductReviews.Attach(review);
            }

            var isDeletedProperty = review.GetType().GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.CanWrite)
            {
                isDeletedProperty.SetValue(review, true);
            }

            _dbContext.ProductReviews.Update(review);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // متدهای جدید برای مدیریت بررسی‌ها
        public async Task<ProductReview> AddReviewAsync(ProductReview review, CancellationToken cancellationToken = default)
        {
            await _dbContext.ProductReviews.AddAsync(review, cancellationToken);
            return review;
        }

        public async Task RemoveByProductIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            var reviews = await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && !pr.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var review in reviews)
            {
                var isDeletedProperty = review.GetType().GetProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.CanWrite)
                {
                    isDeletedProperty.SetValue(review, true);
                }
                _dbContext.ProductReviews.Update(review);
            }
        }

        public async Task<(IEnumerable<ProductReview> Reviews, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            int? productId = null,
            string userId = null,
            ReviewStatus? status = null,
            int? minRating = null,
            int? maxRating = null,
            bool onlyVerified = false,
            string sortBy = null,
            bool sortAscending = true,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.ProductReviews.Where(pr => !pr.IsDeleted);

            if (productId.HasValue)
            {
                query = query.Where(pr => pr.ProductId == productId.Value);
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(pr => pr.UserId == userId);
            }

            if (status.HasValue)
            {
                query = query.Where(pr => pr.Status == status.Value);
            }

            if (minRating.HasValue)
            {
                query = query.Where(pr => pr.Rating >= minRating.Value);
            }

            if (maxRating.HasValue)
            {
                query = query.Where(pr => pr.Rating <= maxRating.Value);
            }

            if (onlyVerified)
            {
                query = query.Where(pr => pr.IsVerifiedPurchase);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            query = sortBy switch
            {
                "rating" => sortAscending ? query.OrderBy(pr => pr.Rating) : query.OrderByDescending(pr => pr.Rating),
                "date" => sortAscending ? query.OrderBy(pr => pr.CreatedAt) : query.OrderByDescending(pr => pr.CreatedAt),
                "helpful" => sortAscending ? query.OrderBy(pr => pr.HelpfulVotes) : query.OrderByDescending(pr => pr.HelpfulVotes),
                _ => query.OrderByDescending(pr => pr.CreatedAt)
            };

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(pr => pr.Product)
                .ToListAsync(cancellationToken);

            return (reviews, totalCount);
        }

        public async Task<IEnumerable<ProductReview>> GetPendingReviewsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.Status == ReviewStatus.Pending && !pr.IsDeleted)
                .Include(pr => pr.Product)
                .OrderBy(pr => pr.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetApprovedReviewsAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .Include(pr => pr.Product)
                .OrderByDescending(pr => pr.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetRejectedReviewsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.Status == ReviewStatus.Rejected && !pr.IsDeleted)
                .Include(pr => pr.Product)
                .OrderByDescending(pr => pr.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task ApproveAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            var review = await _dbContext.ProductReviews.FindAsync(new object[] { reviewId }, cancellationToken);
            if (review != null && !review.IsDeleted)
            {
                review.Approve();
                _dbContext.ProductReviews.Update(review);
            }
        }

        public async Task RejectAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            var review = await _dbContext.ProductReviews.FindAsync(new object[] { reviewId }, cancellationToken);
            if (review != null && !review.IsDeleted)
            {
                review.Reject();
                _dbContext.ProductReviews.Update(review);
            }
        }

        public async Task MarkAsVerifiedAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            var review = await _dbContext.ProductReviews.FindAsync(new object[] { reviewId }, cancellationToken);
            if (review != null && !review.IsDeleted)
            {
                review.MarkAsVerifiedPurchase();
                _dbContext.ProductReviews.Update(review);
            }
        }

        public async Task IncrementHelpfulVotesAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            var review = await _dbContext.ProductReviews.FindAsync(new object[] { reviewId }, cancellationToken);
            if (review != null && !review.IsDeleted)
            {
                review.IncrementHelpfulVotes();
                _dbContext.ProductReviews.Update(review);
            }
        }

        public async Task DecrementHelpfulVotesAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            var review = await _dbContext.ProductReviews.FindAsync(new object[] { reviewId }, cancellationToken);
            if (review != null && !review.IsDeleted && review.HelpfulVotes > 0)
            {
                var propertyInfo = review.GetType().GetProperty("HelpfulVotes", BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    var currentValue = (int)propertyInfo.GetValue(review);
                    propertyInfo.SetValue(review, currentValue - 1);
                    _dbContext.ProductReviews.Update(review);
                }
            }
        }

        public async Task<IEnumerable<ProductReview>> GetMostHelpfulReviewsAsync(int productId, int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .OrderByDescending(pr => pr.HelpfulVotes)
                .ThenByDescending(pr => pr.CreatedAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews.AnyAsync(pr => pr.Id == id && !pr.IsDeleted, cancellationToken);
        }

        public async Task<bool> UserHasReviewedProductAsync(string userId, int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews.AnyAsync(pr => pr.UserId == userId && pr.ProductId == productId && !pr.IsDeleted, cancellationToken);
        }

        public async Task<ProductReview> GetUserReviewForProductAsync(string userId, int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Include(pr => pr.Product)
                .FirstOrDefaultAsync(pr => pr.UserId == userId && pr.ProductId == productId && !pr.IsDeleted, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews.CountAsync(pr => !pr.IsDeleted, cancellationToken);
        }

        public async Task<int> CountByProductIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews.CountAsync(pr => pr.ProductId == productId && !pr.IsDeleted, cancellationToken);
        }

        public async Task<int> CountByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews.CountAsync(pr => pr.UserId == userId && !pr.IsDeleted, cancellationToken);
        }

        public async Task<int> CountByStatusAsync(ReviewStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews.CountAsync(pr => pr.Status == status && !pr.IsDeleted, cancellationToken);
        }

        public async Task<double> GetAverageRatingAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .AverageAsync(pr => pr.Rating, cancellationToken);
        }

        public async Task<int> GetTotalReviewsCountAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .CountAsync(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted, cancellationToken);
        }

        public async Task<int> GetVerifiedReviewsCountAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .CountAsync(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && pr.IsVerifiedPurchase && !pr.IsDeleted, cancellationToken);
        }

        public async Task<IDictionary<int, int>> GetRatingDistributionAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .GroupBy(pr => pr.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Rating, x => x.Count, cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetRecentReviewsAsync(int productId, int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .OrderByDescending(pr => pr.CreatedAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetTopRatedReviewsAsync(int productId, int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .OrderByDescending(pr => pr.Rating)
                .ThenByDescending(pr => pr.HelpfulVotes)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetLowestRatedReviewsAsync(int productId, int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .OrderBy(pr => pr.Rating)
                .ThenByDescending(pr => pr.HelpfulVotes)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetMostHelpfulReviewsByUserAsync(string userId, int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.UserId == userId && pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .OrderByDescending(pr => pr.HelpfulVotes)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IDictionary<string, double>> GetAverageRatingByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .SelectMany(p => p.Reviews)
                .Where(pr => pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .GroupBy(pr => pr.Product.Category.Name)
                .Select(g => new { CategoryName = g.Key, AverageRating = g.Average(pr => pr.Rating) })
                .ToDictionaryAsync(x => x.CategoryName, x => x.AverageRating, cancellationToken);
        }

        public async Task<IDictionary<string, double>> GetAverageRatingByBrandAsync(int brandId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.BrandId == brandId && !p.IsDeleted)
                .SelectMany(p => p.Reviews)
                .Where(pr => pr.Status == ReviewStatus.Approved && !pr.IsDeleted)
                .GroupBy(pr => pr.Product.Brand.Name)
                .Select(g => new { BrandName = g.Key, AverageRating = g.Average(pr => pr.Rating) })
                .ToDictionaryAsync(x => x.BrandName, x => x.AverageRating, cancellationToken);
        }
    }
}