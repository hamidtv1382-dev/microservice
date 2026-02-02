using Catalog_Service.src._01_Domain.Core.Contracts.Repositories;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Catalog_Service.src._01_Domain.Core.Primitives;
using Catalog_Service.src._02_Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace Catalog_Service.src._02_Infrastructure.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
        }

        public async Task<Category> GetBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Slug == slug && !c.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Category>> GetAllWithSubCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.SubCategories)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Where(c => c.IsActive && !c.IsDeleted)
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .ToListAsync(cancellationToken);
        }

        public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await _dbContext.Categories.AddAsync(category, cancellationToken);
            return category;
        }

        public void Update(Category category)
        {
            _dbContext.Categories.Update(category);
        }

        public void Remove(Category category)
        {
            // Soft Delete Implementation
            var entry = _dbContext.Entry(category);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.Categories.Attach(category);
            }

            // دسترسی به پراپرتی IsDeleted برای تغییر وضعیت
            var isDeletedProperty = category.GetType().GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.CanWrite)
            {
                isDeletedProperty.SetValue(category, true);
            }

            _dbContext.Categories.Update(category);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Where(c => c.ParentCategoryId == null && !c.IsDeleted)
                .Include(c => c.SubCategories)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Where(c => c.ParentCategoryId == parentCategoryId && !c.IsDeleted)
                .Include(c => c.SubCategories)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllDescendantsAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var descendants = new List<Category>();
            var queue = new Queue<int>();
            queue.Enqueue(categoryId);

            while (queue.Count > 0)
            {
                var currentId = queue.Dequeue();
                var children = await _dbContext.Categories
                    .Where(c => c.ParentCategoryId == currentId && !c.IsDeleted)
                    .ToListAsync(cancellationToken);

                foreach (var child in children)
                {
                    descendants.Add(child);
                    queue.Enqueue(child.Id);
                }
            }

            return descendants;
        }

        public async Task<Category> GetParentCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted, cancellationToken);

            // اگر خود والد حذف شده باشد، نباید برگردانده شود (یا بسته به منطق کسب‌وکار ممکن است null برگردد)
            // اینجا فرض می‌کنیم اگر والد در دیتابیس وجود داشته باشد حتی اگر حذف شده باشد آبجکت برمی‌گردد، اما معمولاً نباید.
            // برای سادگی، همان والد را برمی‌گردانیم، اما در کوئری بالا والد فیلتر نشده است، فقط خود فرزند.
            return category?.ParentCategory;
        }

        public async Task<IEnumerable<Category>> GetCategoryPathAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var path = new List<Category>();
            int? currentId = categoryId;

            while (currentId.HasValue)
            {
                var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == currentId.Value && !c.IsDeleted, cancellationToken);

                if (category != null)
                {
                    path.Insert(0, category);
                    currentId = category.ParentCategoryId;
                }
                else
                {
                    break;
                }
            }

            return path;
        }

        public async Task<bool> HasSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories.AnyAsync(c => c.ParentCategoryId == categoryId && !c.IsDeleted, cancellationToken);
        }

        public async Task<bool> HasProductsAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.AnyAsync(p => p.CategoryId == categoryId && !p.IsDeleted, cancellationToken);
        }

        public async Task<(IEnumerable<Category> Categories, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string searchTerm = null,
            bool onlyActive = true,
            int? parentCategoryId = null,
            string sortBy = null,
            bool sortAscending = true,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Categories.Where(c => !c.IsDeleted);

            if (onlyActive)
            {
                query = query.Where(c => c.IsActive);
            }

            if (parentCategoryId.HasValue)
            {
                query = query.Where(c => c.ParentCategoryId == parentCategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            query = sortBy switch
            {
                "name" => sortAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                "displayOrder" => sortAscending ? query.OrderBy(c => c.DisplayOrder) : query.OrderByDescending(c => c.DisplayOrder),
                "date" => sortAscending ? query.OrderBy(c => c.CreatedAt) : query.OrderByDescending(c => c.CreatedAt),
                _ => query.OrderBy(c => c.DisplayOrder)
            };

            var categories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .ToListAsync(cancellationToken);

            return (categories, totalCount);
        }

        public async Task<IEnumerable<Category>> GetOrderedByDisplayOrderAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetMaxDisplayOrderAsync(int? parentCategoryId = null, CancellationToken cancellationToken = default)
        {
            var maxOrder = await _dbContext.Categories
                .Where(c => c.ParentCategoryId == parentCategoryId && !c.IsDeleted)
                .Select(c => (int?)c.DisplayOrder)
                .MaxAsync(cancellationToken);

            return maxOrder ?? 0;
        }

        public async Task UpdateDisplayOrderAsync(int categoryId, int displayOrder, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories.FindAsync(new object[] { categoryId }, cancellationToken);
            if (category != null && !category.IsDeleted)
            {
                category.SetDisplayOrder(displayOrder);
                _dbContext.Categories.Update(category);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
        }

        public async Task<bool> ExistsBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Slug == slug && !c.IsDeleted, cancellationToken);
        }

        public async Task<bool> IsUniqueSlugAsync(Slug slug, int? excludeCategoryId = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Categories.Where(c => c.Slug == slug && !c.IsDeleted);

            if (excludeCategoryId.HasValue)
            {
                query = query.Where(c => c.Id != excludeCategoryId.Value);
            }

            return !await query.AnyAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories.CountAsync(c => !c.IsDeleted, cancellationToken);
        }

        public async Task<int> CountActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories.CountAsync(c => c.IsActive && !c.IsDeleted, cancellationToken);
        }

        public async Task<int> CountSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories.CountAsync(c => c.ParentCategoryId == categoryId && !c.IsDeleted, cancellationToken);
        }

        public async Task<int> CountProductsAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.CountAsync(p => p.CategoryId == categoryId && !p.IsDeleted, cancellationToken);
        }

        public async Task<bool> IsAncestorOfAsync(int ancestorId, int descendantId, CancellationToken cancellationToken = default)
        {
            int? currentId = descendantId;

            while (currentId.HasValue)
            {
                if (currentId.Value == ancestorId)
                    return true;

                currentId = await _dbContext.Categories
                    .Where(c => c.Id == currentId.Value && !c.IsDeleted)
                    .Select(c => c.ParentCategoryId)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            return false;
        }

        public async Task<bool> IsDescendantOfAsync(int descendantId, int ancestorId, CancellationToken cancellationToken = default)
        {
            return await IsAncestorOfAsync(ancestorId, descendantId, cancellationToken);
        }

        public async Task<bool> WouldCreateCircularReferenceAsync(int parentId, int childId, CancellationToken cancellationToken = default)
        {
            if (parentId == childId)
                return true;

            return await IsAncestorOfAsync(childId, parentId, cancellationToken);
        }

        public async Task MoveCategoryAsync(int categoryId, int? newParentId, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories.FindAsync(new object[] { categoryId }, cancellationToken);
            if (category != null && !category.IsDeleted)
            {
                if (newParentId.HasValue && await WouldCreateCircularReferenceAsync(newParentId.Value, categoryId, cancellationToken))
                {
                    throw new InvalidOperationException("Moving this category would create a circular reference.");
                }

                category.SetParentCategory(newParentId);
                _dbContext.Categories.Update(category);
            }
        }

        public async Task ActivateAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories.FindAsync(new object[] { categoryId }, cancellationToken);
            if (category != null && !category.IsDeleted)
            {
                category.Activate();
                _dbContext.Categories.Update(category);
            }
        }

        public async Task DeactivateAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories.FindAsync(new object[] { categoryId }, cancellationToken);
            if (category != null && !category.IsDeleted)
            {
                category.Deactivate();
                _dbContext.Categories.Update(category);
            }
        }

        public async Task ActivateWithSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var categoryIds = new List<int> { categoryId };
            var descendants = await GetAllDescendantsAsync(categoryId, cancellationToken);
            categoryIds.AddRange(descendants.Select(d => d.Id));

            await _dbContext.Categories
                .Where(c => categoryIds.Contains(c.Id) && !c.IsDeleted)
                .ForEachAsync(c => c.Activate(), cancellationToken);
        }

        public async Task DeactivateWithSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var categoryIds = new List<int> { categoryId };
            var descendants = await GetAllDescendantsAsync(categoryId, cancellationToken);
            categoryIds.AddRange(descendants.Select(d => d.Id));

            await _dbContext.Categories
                .Where(c => categoryIds.Contains(c.Id) && !c.IsDeleted)
                .ForEachAsync(c => c.Deactivate(), cancellationToken);
        }

        public async Task<int> GetTotalProductsCountAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var categoryIds = new List<int> { categoryId };
            var descendants = await GetAllDescendantsAsync(categoryId, cancellationToken);
            categoryIds.AddRange(descendants.Select(d => d.Id));

            return await _dbContext.Products
                .CountAsync(p => categoryIds.Contains(p.CategoryId) && !p.IsDeleted, cancellationToken);
        }

        public async Task<int> GetActiveProductsCountAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var categoryIds = new List<int> { categoryId };
            var descendants = await GetAllDescendantsAsync(categoryId, cancellationToken);
            categoryIds.AddRange(descendants.Select(d => d.Id));

            return await _dbContext.Products
                .CountAsync(p => categoryIds.Contains(p.CategoryId) && p.Status == ProductStatus.Published && !p.IsDeleted, cancellationToken);
        }

        public async Task<int> GetTotalSubCategoriesCountAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .CountAsync(c => c.ParentCategoryId == categoryId && !c.IsDeleted, cancellationToken);
        }
    }
}