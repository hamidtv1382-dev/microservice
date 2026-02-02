using Catalog_Service.src._01_Domain.Core.Contracts.Repositories;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._02_Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog_Service.src._02_Infrastructure.Data.Repositories
{
    public class ProductAttributeRepository : IProductAttributeRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductAttributeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductAttribute> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Include(pa => pa.Product)
                .Include(pa => pa.ProductVariant)
                .FirstOrDefaultAsync(pa => pa.Id == id && !pa.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<ProductAttribute>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => !pa.IsDeleted)
                .Include(pa => pa.Product)
                .Include(pa => pa.ProductVariant)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductAttribute>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Include(pa => pa.Product)
                .Include(pa => pa.ProductVariant)
                .Where(pa => pa.ProductId == productId && !pa.IsVariantSpecific && !pa.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductAttribute>> GetByProductVariantIdAsync(int productVariantId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Include(pa => pa.Product)
                .Include(pa => pa.ProductVariant)
                .Where(pa => pa.ProductVariantId == productVariantId && !pa.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductAttribute> AddAsync(ProductAttribute attribute, CancellationToken cancellationToken = default)
        {
            await _dbContext.ProductAttributes.AddAsync(attribute, cancellationToken);
            return attribute;
        }

        public void Update(ProductAttribute attribute)
        {
            _dbContext.ProductAttributes.Update(attribute);
        }

        public void Remove(ProductAttribute attribute)
        {
            // Soft Delete Implementation
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

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // پیاده‌سازی متد جدید
        public async Task<IEnumerable<ProductAttribute>> GetAttributesAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && !pa.IsDeleted)
                .OrderBy(pa => pa.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<ProductAttribute> Attributes, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            int? productId = null,
            int? productVariantId = null,
            string name = null,
            bool onlyVariantSpecific = false,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.ProductAttributes.Where(pa => !pa.IsDeleted);

            if (productId.HasValue)
            {
                query = query.Where(pa => pa.ProductId == productId.Value);
            }

            if (productVariantId.HasValue)
            {
                query = query.Where(pa => pa.ProductVariantId == productVariantId.Value);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(pa => pa.Name.Contains(name));
            }

            if (onlyVariantSpecific)
            {
                query = query.Where(pa => pa.IsVariantSpecific);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var attributes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(pa => pa.Product)
                .Include(pa => pa.ProductVariant)
                .OrderBy(pa => pa.Name)
                .ToListAsync(cancellationToken);

            return (attributes, totalCount);
        }

        public async Task<IEnumerable<ProductAttribute>> GetProductAttributesAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && !pa.IsVariantSpecific && !pa.IsDeleted)
                .OrderBy(pa => pa.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductAttribute>> GetVariantAttributesAsync(int productVariantId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductVariantId == productVariantId && !pa.IsDeleted)
                .OrderBy(pa => pa.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetUniqueAttributeNamesAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && !pa.IsDeleted)
                .Select(pa => pa.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetUniqueAttributeValuesAsync(int productId, string attributeName, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && pa.Name == attributeName && !pa.IsDeleted)
                .Select(pa => pa.Value)
                .Distinct()
                .OrderBy(value => value)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductAttribute>> GetCommonAttributesAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => productIds.Contains(pa.ProductId) && !pa.IsVariantSpecific && !pa.IsDeleted)
                .GroupBy(pa => pa.Name)
                .Where(g => g.Count() == productIds.Count())
                .SelectMany(g => g)
                .OrderBy(pa => pa.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductAttribute>> GetDistinctAttributesAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && !pa.IsDeleted)
                .GroupBy(pa => new { pa.Name, pa.Value })
                .Select(g => g.First())
                .OrderBy(pa => pa.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes.AnyAsync(pa => pa.Id == id && !pa.IsDeleted, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes.CountAsync(pa => !pa.IsDeleted, cancellationToken);
        }

        public async Task<int> CountByProductIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes.CountAsync(pa => pa.ProductId == productId && !pa.IsDeleted, cancellationToken);
        }

        public async Task<int> CountByProductVariantIdAsync(int productVariantId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes.CountAsync(pa => pa.ProductVariantId == productVariantId && !pa.IsDeleted, cancellationToken);
        }

        public async Task RemoveAllProductAttributesAsync(int productId, CancellationToken cancellationToken = default)
        {
            var attributes = await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && !pa.IsVariantSpecific && !pa.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var attribute in attributes)
            {
                var isDeletedProperty = attribute.GetType().GetProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.CanWrite)
                {
                    isDeletedProperty.SetValue(attribute, true);
                }
                _dbContext.ProductAttributes.Update(attribute);
            }
        }

        public async Task RemoveAllVariantAttributesAsync(int productVariantId, CancellationToken cancellationToken = default)
        {
            var attributes = await _dbContext.ProductAttributes
                .Where(pa => pa.ProductVariantId == productVariantId && !pa.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var attribute in attributes)
            {
                var isDeletedProperty = attribute.GetType().GetProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.CanWrite)
                {
                    isDeletedProperty.SetValue(attribute, true);
                }
                _dbContext.ProductAttributes.Update(attribute);
            }
        }

        public async Task RemoveAllAttributesByNameAsync(int productId, string attributeName, CancellationToken cancellationToken = default)
        {
            var attributes = await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && pa.Name == attributeName && !pa.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var attribute in attributes)
            {
                var isDeletedProperty = attribute.GetType().GetProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.CanWrite)
                {
                    isDeletedProperty.SetValue(attribute, true);
                }
                _dbContext.ProductAttributes.Update(attribute);
            }
        }

        public async Task UpdateAllAttributesValueAsync(int productId, string attributeName, string newValue, CancellationToken cancellationToken = default)
        {
            var attributes = await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && pa.Name == attributeName && !pa.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var attribute in attributes)
            {
                attribute.UpdateValue(newValue);
                _dbContext.ProductAttributes.Update(attribute);
            }
        }

        public async Task UpdateAllVariantAttributesValueAsync(int productVariantId, string attributeName, string newValue, CancellationToken cancellationToken = default)
        {
            var attributes = await _dbContext.ProductAttributes
                .Where(pa => pa.ProductVariantId == productVariantId && pa.Name == attributeName && !pa.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var attribute in attributes)
            {
                attribute.UpdateValue(newValue);
                _dbContext.ProductAttributes.Update(attribute);
            }
        }

        public async Task CopyProductAttributesToVariantAsync(int productId, int productVariantId, CancellationToken cancellationToken = default)
        {
            var productAttributes = await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && !pa.IsVariantSpecific && !pa.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var attribute in productAttributes)
            {
                var variantAttribute = new ProductAttribute(
                    productId,
                    attribute.Name,
                    attribute.Value,
                    productVariantId,
                    true);

                await _dbContext.ProductAttributes.AddAsync(variantAttribute, cancellationToken);
            }
        }

        public async Task CopyVariantAttributesToProductAsync(int productVariantId, int productId, CancellationToken cancellationToken = default)
        {
            var variantAttributes = await _dbContext.ProductAttributes
                .Where(pa => pa.ProductVariantId == productVariantId && !pa.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var attribute in variantAttributes)
            {
                var productAttribute = new ProductAttribute(
                    productId,
                    attribute.Name,
                    attribute.Value,
                    null,
                    false);

                await _dbContext.ProductAttributes.AddAsync(productAttribute, cancellationToken);
            }
        }

        public async Task CopyAttributesBetweenProductsAsync(int sourceProductId, int targetProductId, CancellationToken cancellationToken = default)
        {
            var sourceAttributes = await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == sourceProductId && !pa.IsVariantSpecific && !pa.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var attribute in sourceAttributes)
            {
                var targetAttribute = new ProductAttribute(
                    targetProductId,
                    attribute.Name,
                    attribute.Value,
                    null,
                    false);

                await _dbContext.ProductAttributes.AddAsync(targetAttribute, cancellationToken);
            }
        }

        public async Task<IEnumerable<ProductAttribute>> GetAttributesContainingValueAsync(int productId, string searchValue, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && pa.Value.Contains(searchValue) && !pa.IsDeleted)
                .OrderBy(pa => pa.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductAttribute>> GetAttributesByNameAsync(int productId, string attributeName, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && pa.Name == attributeName && !pa.IsDeleted)
                .OrderBy(pa => pa.Value)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductAttribute>> GetAttributesByValueAsync(int productId, string attributeValue, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductAttributes
                .Where(pa => pa.ProductId == productId && pa.Value == attributeValue && !pa.IsDeleted)
                .OrderBy(pa => pa.Name)
                .ToListAsync(cancellationToken);
        }
    }
}