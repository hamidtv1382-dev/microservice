using Catalog_Service.src._01_Domain.Core.Contracts.Repositories;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Catalog_Service.src._01_Domain.Core.Primitives;
using Catalog_Service.src._02_Infrastructure.Data.Db;
using Catalog_Service.src.CrossCutting.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog_Service.src._02_Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Include(p => p.Attributes)
                .Include(p => p.Reviews)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted && p.IsApproved, cancellationToken);
        }

        public async Task<Product> GetByIdVendorAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Include(p => p.Attributes)
                .Include(p => p.Reviews)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
        }

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Sku == sku && !p.IsDeleted && p.IsApproved, cancellationToken);
        }


        public async Task<Product> GetBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Include(p => p.Attributes)
                .Include(p => p.Reviews)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Slug == slug && !p.IsDeleted && p.IsApproved, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
            return product;
        }

        public void Update(Product product)
        {
            _dbContext.Products.Update(product);
        }

        public void Remove(Product product)
        {
            // Soft Delete Implementation
            var entry = _dbContext.Entry(product);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.Products.Attach(product);
            }

            var isDeletedProperty = product.GetType().GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.CanWrite)
            {
                isDeletedProperty.SetValue(product, true);
            }

            _dbContext.Products.Update(product);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // --- متدهای جدید ادمین ---

        public async Task<IEnumerable<Product>> GetAllForAdminAsync(CancellationToken cancellationToken = default)
        {
            // ادمین تمام محصولات را می‌بیند، حتی تایید نشده‌ها (اما نه حذف شده‌ها)
            return await _dbContext.Products
                .Where(p => !p.IsDeleted) // فقط آیتم‌های حذف شده فیلتر می‌شوند
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task SetApprovalStatusAsync(int productId, bool isApproved, CancellationToken cancellationToken = default)
        {
            // دریافت محصول از دیتابیس
            var product = await _dbContext.Products.FindAsync(new object[] { productId }, cancellationToken);

            // اگر محصول وجود داشت و حذف نشده بود
            if (product != null && !product.IsDeleted)
            {
                // اگر setter عمومی است، مستقیم مقدار بده
                var property = product.GetType().GetProperty("IsApproved");
                if (property != null && property.CanWrite)
                {
                    property.SetValue(product, isApproved);
                }

                // ذخیره تغییرات در دیتابیس
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NotFoundException("Product", productId);
            }
        }


        // -------------------------------

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.CategoryId == categoryId && p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.BrandId == brandId && p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByPriceRangeAsync(Money minPrice, Money maxPrice, CancellationToken cancellationToken = default)
        {
            // نکته: این متد از Value Object مستقیماً استفاده می‌کند که ممکن است در SQL مشکل ساز شود
            // اما چون معمولاً رنج محدود است، فعلاً به همین شکل باقی می‌ماند.
            // اگر خطا داد، باید مثل GetPagedAsync به حافظه منتقل شود.
            return await _dbContext.Products
                .Where(p => p.Price.Amount >= minPrice.Amount && p.Price.Amount <= maxPrice.Amount && p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.IsFeatured && p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetNewestProductsAsync(int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetBestSellingProductsAsync(int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(
       int pageNumber,
       int pageSize,
       string searchTerm = null,
       int? categoryId = null,
       int? brandId = null,
       ProductStatus? status = null,
       decimal? minPrice = null,
       decimal? maxPrice = null,
       string sortBy = null,
       bool sortAscending = true,
       CancellationToken cancellationToken = default)
        {
            // مرحله ۱: ساخت کوئری پایه با فیلترهای قابل ترجمه برای دیتابیس
            var dbQuery = _dbContext.Products
                .Where(p => !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                dbQuery = dbQuery.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                dbQuery = dbQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                dbQuery = dbQuery.Where(p => p.BrandId == brandId.Value);
            }

            if (status.HasValue)
            {
                dbQuery = dbQuery.Where(p => p.Status == status.Value);
            }

            // مرحله ۲: خواندن نتایج فیلتر شده از دیتابیس به حافظه
            var filteredProductsFromDb = await dbQuery.ToListAsync(cancellationToken);

            // مرحله ۳: اعمال فیلتر قیمت و مرتب‌سازی در حافظه
            IEnumerable<Product> finalQuery = filteredProductsFromDb;

            if (minPrice.HasValue)
            {
                finalQuery = finalQuery.Where(p => p.Price.Amount >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                finalQuery = finalQuery.Where(p => p.Price.Amount <= maxPrice.Value);
            }

            // حالا مرتب‌سازی را در حافظه انجام دهید
            finalQuery = sortBy switch
            {
                "name" => sortAscending ? finalQuery.OrderBy(p => p.Name) : finalQuery.OrderByDescending(p => p.Name),
                "price" => sortAscending ? finalQuery.OrderBy(p => p.Price.Amount) : finalQuery.OrderByDescending(p => p.Price.Amount),
                "date" => sortAscending ? finalQuery.OrderBy(p => p.CreatedAt) : finalQuery.OrderByDescending(p => p.CreatedAt),
                "rating" => sortAscending ?
                    finalQuery.OrderBy(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0) :
                    finalQuery.OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0),
                _ => finalQuery.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = finalQuery.Count();
            var products = finalQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return (products, totalCount);
        }


        public async Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.Status == status && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.StockStatus == StockStatus.OutOfStock && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.StockQuantity <= threshold && p.StockQuantity > 0 && !p.IsDeleted && p.IsApproved)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.AnyAsync(p => p.Id == id && !p.IsDeleted && p.IsApproved, cancellationToken);
        }

        public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.AnyAsync(p => p.Sku == sku && !p.IsDeleted && p.IsApproved, cancellationToken);
        }

        public async Task<bool> ExistsBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.AnyAsync(p => p.Slug == slug && !p.IsDeleted && p.IsApproved, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.CountAsync(p => !p.IsDeleted && p.IsApproved, cancellationToken);
        }

        public async Task<int> CountByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.CountAsync(p => p.CategoryId == categoryId && !p.IsDeleted && p.IsApproved, cancellationToken);
        }

        public async Task<int> CountByBrandAsync(int brandId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.CountAsync(p => p.BrandId == brandId && !p.IsDeleted && p.IsApproved, cancellationToken);
        }

        public async Task UpdateStockQuantityAsync(int productId, int quantity, CancellationToken cancellationToken = default)
        {
            var product = await _dbContext.Products.FindAsync(new object[] { productId }, cancellationToken);
            if (product != null && !product.IsDeleted)
            {
                product.UpdateStock(quantity);
                _dbContext.Products.Update(product);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateStockStatusAsync(int productId, StockStatus status, CancellationToken cancellationToken = default)
        {
            var product = await _dbContext.Products.FindAsync(new object[] { productId }, cancellationToken);
            if (product != null && !product.IsDeleted)
            {
                product.SetStockStatus(status);
                _dbContext.Products.Update(product);
            }
        }

        public async Task AddVariantAsync(ProductVariant variant, CancellationToken cancellationToken = default)
        {
            await _dbContext.ProductVariants.AddAsync(variant, cancellationToken);
        }

        public async Task RemoveVariantAsync(ProductVariant variant, CancellationToken cancellationToken = default)
        {
            // Soft Delete
            var entry = _dbContext.Entry(variant);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.ProductVariants.Attach(variant);
            }

            var isDeletedProperty = variant.GetType().GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.CanWrite)
            {
                isDeletedProperty.SetValue(variant, true);
            }

            _dbContext.ProductVariants.Update(variant);
        }

        public async Task AddImageAsync(ImageResource image, CancellationToken cancellationToken = default)
        {
            await _dbContext.ImageResources.AddAsync(image, cancellationToken);
        }

        public async Task RemoveImageAsync(ImageResource image, CancellationToken cancellationToken = default)
        {
            // Soft Delete
            var entry = _dbContext.Entry(image);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.ImageResources.Attach(image);
            }

            var isDeletedProperty = image.GetType().GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.CanWrite)
            {
                isDeletedProperty.SetValue(image, true);
            }

            _dbContext.ImageResources.Update(image);
        }

        public async Task AddAttributeAsync(ProductAttribute attribute, CancellationToken cancellationToken = default)
        {
            await _dbContext.ProductAttributes.AddAsync(attribute, cancellationToken);
        }

        public async Task RemoveAttributeAsync(ProductAttribute attribute, CancellationToken cancellationToken = default)
        {
            // Soft Delete
            var entry = _dbContext.Entry(attribute);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.ProductAttributes.Attach(attribute);
            }

            var isDeletedProperty = attribute.GetType().GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.CanWrite)
            {
                isDeletedProperty.SetValue(attribute, true);
            }

            _dbContext.ProductAttributes.Update(attribute);
        }

        public async Task AddTagAsync(ProductTag tag, CancellationToken cancellationToken = default)
        {
            await _dbContext.ProductTags.AddAsync(tag, cancellationToken);
        }

        public async Task RemoveTagAsync(ProductTag tag, CancellationToken cancellationToken = default)
        {
            // Soft Delete
            var entry = _dbContext.Entry(tag);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.ProductTags.Attach(tag);
            }

            var isDeletedProperty = tag.GetType().GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.CanWrite)
            {
                isDeletedProperty.SetValue(tag, true);
            }

            _dbContext.ProductTags.Update(tag);
        }

        public async Task AddReviewAsync(ProductReview review, CancellationToken cancellationToken = default)
        {
            await _dbContext.ProductReviews.AddAsync(review, cancellationToken);
        }

        public async Task RemoveReviewAsync(ProductReview review, CancellationToken cancellationToken = default)
        {
            // Soft Delete
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

        public async Task<decimal> GetAveragePriceByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.CategoryId == categoryId && p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .AverageAsync(p => p.Price.Amount, cancellationToken);
        }

        public async Task<decimal> GetAveragePriceByBrandAsync(int brandId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Where(p => p.BrandId == brandId && p.Status == ProductStatus.Published && !p.IsDeleted && p.IsApproved)
                .AverageAsync(p => p.Price.Amount, cancellationToken);
        }

        public async Task<int> GetViewCountAsync(int productId, CancellationToken cancellationToken = default)
        {
            var product = await _dbContext.Products.FindAsync(new object[] { productId }, cancellationToken);
            return (product != null && !product.IsDeleted) ? product.ViewCount : 0;
        }

        public async Task IncrementViewCountAsync(int productId, CancellationToken cancellationToken = default)
        {
            var product = await _dbContext.Products.FindAsync(new object[] { productId }, cancellationToken);
            if (product != null && !product.IsDeleted)
            {
                product.IncrementViewCount();
                _dbContext.Products.Update(product);
            }
        }

        public async Task<double> GetAverageRatingAsync(int productId, CancellationToken cancellationToken = default)
        {
            var approvedReviews = await _dbContext.ProductReviews
                .Where(r => r.ProductId == productId && r.Status == ReviewStatus.Approved && !r.IsDeleted)
                .Select(r => r.Rating)
                .ToListAsync(cancellationToken);

            if (!approvedReviews.Any())
                return 0.0;

            return approvedReviews.Average();
        }

        public async Task<int> GetTotalReviewsCountAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductReviews
                .Where(r => r.ProductId == productId && r.Status == ReviewStatus.Approved && !r.IsDeleted)
                .CountAsync(cancellationToken);
        }

        public async Task<IDictionary<int, int>> GetRatingDistributionAsync(int productId, CancellationToken cancellationToken = default)
        {
            var distribution = await _dbContext.ProductReviews
                .Where(r => r.ProductId == productId && r.Status == ReviewStatus.Approved && !r.IsDeleted)
                .GroupBy(r => r.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Rating, x => x.Count, cancellationToken);

            for (int i = 1; i <= 5; i++)
            {
                if (!distribution.ContainsKey(i))
                {
                    distribution[i] = 0;
                }
            }

            return distribution;
        }
    }
}