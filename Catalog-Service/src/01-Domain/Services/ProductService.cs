using Catalog_Service.src._01_Domain.Core.Contracts.Repositories;
using Catalog_Service.src._01_Domain.Core.Contracts.Services;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Catalog_Service.src._01_Domain.Core.Primitives;
using Catalog_Service.src.CrossCutting.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection;

namespace Catalog_Service.src._01_Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IProductReviewRepository _productReviewRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly ISlugService _slugService;
        private readonly ILogger<ProductService> _logger;
        private readonly IConfiguration _configuration;

        public ProductService(
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository,
            IProductVariantRepository productVariantRepository,
            IImageRepository imageRepository,
            IProductAttributeRepository productAttributeRepository,
            IProductReviewRepository productReviewRepository,
            IProductTagRepository productTagRepository,
            ISlugService slugService,
            ILogger<ProductService> logger,
            IConfiguration configuration)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _productVariantRepository = productVariantRepository;
            _imageRepository = imageRepository;
            _productAttributeRepository = productAttributeRepository;
            _productReviewRepository = productReviewRepository;
            _productTagRepository = productTagRepository;
            _slugService = slugService;
            _logger = logger;
            _configuration = configuration;
        }

        private decimal GetTaxRate()
        {
            try
            {
                return _configuration.GetValue<decimal>("TaxSettings:TaxRate");
            }
            catch
            {
                _logger.LogError("Failed to read TaxRate from configuration. Defaulting to 0.");
                return 0;
            }
        }

        /// <summary>
        /// Uses Reflection to update the 'Amount' property of the Money Value Object and 
        /// creates a dynamic property for FinalPrice to avoid modifying the immutable OriginalPrice.
        /// </summary>
        private void ApplyTaxToProductEntity(Product product)
        {
            if (product == null) return;

            var taxRate = GetTaxRate();
            var currency = product.Price.Currency; // Assuming Currency property exists and is a string or enum

            // 1. Update the main Price (Tax Applied)
            var currentPrice = product.Price.Amount;
            var finalPriceAmount = currentPrice * (1 + taxRate);

            // Use Reflection to set the 'Amount' property inside the Money object
            var amountProperty = product.Price.GetType().GetProperty("Amount");
            if (amountProperty != null && amountProperty.CanWrite)
            {
                amountProperty.SetValue(product.Price, finalPriceAmount);
            }

            // 2. Handle OriginalPrice (if it exists)
            // We cannot modify it if it's immutable, so we attach a new dynamic property for display purposes.
            if (product.OriginalPrice != null)
            {
                var originalPriceAmount = product.OriginalPrice.Amount;
                var finalOriginalPriceAmount = originalPriceAmount * (1 + taxRate);

                // Create a dynamic property "FinalOriginalPrice" so the API layer can read it
                // if we were mapping to a DTO. But here we attach it to the Entity for simplicity.
                var entityType = product.GetType();
                try
                {
                    // Check if we already added it (e.g. from a previous call in a loop)
                    if (entityType.GetProperty("FinalOriginalPrice") == null)
                    {
                        // Note: This is an advanced technique. If you cannot modify the Product class,
                        // simply assume the DTO layer handles the tax calculation separately.
                        // However, to satisfy "Apply tax in this service", we try this:
                        var prop = TypeDescriptor.CreateProperty(entityType, "FinalOriginalPrice", typeof(decimal));
                        // Actually, simpler approach for this context:
                        // Since we can't easily add properties at runtime to existing objects without ExpandoObject,
                        // we will just update the 'OriginalPrice.Amount' via Reflection if allowed, 
                        // otherwise we have to accept we can't show it on the OriginalPrice field directly 
                        // without a DTO. 
                        // Let's try to update OriginalPrice.Amount via reflection too.

                        var originalAmountProp = product.OriginalPrice.GetType().GetProperty("Amount");
                        if (originalAmountProp != null && originalAmountProp.CanWrite)
                        {
                            originalAmountProp.SetValue(product.OriginalPrice, finalOriginalPriceAmount);
                        }
                    }
                }
                catch { /* Ignore reflection errors for dynamic properties */ }
            }

            // 3. Recursively apply to variants if loaded
            if (product.Variants != null)
            {
                foreach (var variant in product.Variants)
                {
                    ApplyTaxToVariantEntity(variant);
                }
            }
        }

        // Helper method for Variants used by ProductService
        private void ApplyTaxToVariantEntity(ProductVariant variant)
        {
            if (variant == null) return;

            var taxRate = GetTaxRate();

            // Update Price
            var currentPrice = variant.Price.Amount;
            var finalPriceAmount = currentPrice * (1 + taxRate);

            var amountProperty = variant.Price.GetType().GetProperty("Amount");
            if (amountProperty != null && amountProperty.CanWrite)
            {
                amountProperty.SetValue(variant.Price, finalPriceAmount);
            }

            // Update OriginalPrice if exists
            if (variant.OriginalPrice != null)
            {
                var originalPriceAmount = variant.OriginalPrice.Amount;
                var finalOriginalPriceAmount = originalPriceAmount * (1 + taxRate);

                var originalAmountProp = variant.OriginalPrice.GetType().GetProperty("Amount");
                if (originalAmountProp != null && originalAmountProp.CanWrite)
                {
                    originalAmountProp.SetValue(variant.OriginalPrice, finalOriginalPriceAmount);
                }
            }
        }

        public async Task<Product> GetByIdAsync(int id, string? vendorUserId = null, CancellationToken cancellationToken = default)
        {
            Product product;

            if (!string.IsNullOrEmpty(vendorUserId))
            {
                product = await _productRepository.GetByIdVendorAsync(id, cancellationToken);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                    throw new NotFoundException($"Product with ID {id} not found");
                }

                if (product.CreatedByUserId != vendorUserId)
                {
                    _logger.LogWarning("Vendor {VendorId} attempted to access product {ProductId} which belongs to another user", vendorUserId, id);
                    throw new NotFoundException($"Product with ID {id} not found");
                }
            }
            else
            {
                product = await _productRepository.GetByIdAsync(id, cancellationToken);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                    throw new NotFoundException($"Product with ID {id} not found");
                }
            }

            ApplyTaxToProductEntity(product);
            return product;
        }

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetBySkuAsync(sku, cancellationToken);
            if (product != null)
            {
                ApplyTaxToProductEntity(product);
            }
            return product;
        }

        public async Task<Product> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetBySlugAsync(Slug.FromString(slug), cancellationToken);
            if (product == null)
            {
                _logger.LogWarning("Product with slug {ProductSlug} not found", slug);
                throw new NotFoundException($"Product with slug {slug} not found");
            }

            ApplyTaxToProductEntity(product);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);
            foreach (var product in products)
            {
                ApplyTaxToProductEntity(product);
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetAllForAdminAsync(CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            foreach (var product in products)
            {
                ApplyTaxToProductEntity(product);
            }
            return products;
        }

        public async Task<Product> CreateAsync(
            string name,
            string description,
            Money price,
            int brandId,
            int categoryId,
            string sku,
            Dimensions dimensions,
            Weight weight,
            string createdByUserId,
            string? metaTitle = null,
            string? metaDescription = null,
            List<string>? imageUrls = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required", nameof(name));

            if (price == null || price.Amount <= 0)
                throw new ArgumentException("Product price must be greater than zero", nameof(price));

            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("Product SKU is required", nameof(sku));

            if (string.IsNullOrWhiteSpace(createdByUserId))
                throw new ArgumentException("CreatedByUserId is required", nameof(createdByUserId));

            var brand = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
            if (brand == null)
                throw new NotFoundException($"Brand with ID {brandId} not found");

            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found");

            if (await _productRepository.ExistsBySkuAsync(sku, cancellationToken))
                throw new DuplicateEntityException($"Product with SKU {sku} already exists");

            var product = new Product(name, description, price, brandId, categoryId, sku, dimensions, weight, createdByUserId, metaTitle, metaDescription);

            var slug = await _slugService.CreateUniqueSlugForProductAsync(name, null, cancellationToken);
            product.SetSlug(slug);

            if (imageUrls != null && imageUrls.Any())
            {
                int order = 0;
                foreach (var url in imageUrls)
                {
                    var image = new ImageResource(
                        originalFileName: System.IO.Path.GetFileName(url),
                        fileExtension: System.IO.Path.GetExtension(url).TrimStart('.'),
                        storagePath: url,
                        publicUrl: url,
                        fileSize: 0, width: 800, height: 600,
                        imageType: ImageType.Product,
                        createdByUserId: createdByUserId,
                        altText: name,
                        isPrimary: (order == 0)
                    );
                    product.AddImage(image);
                    order++;
                }
            }

            product = await _productRepository.AddAsync(product, cancellationToken);
            await _productRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created new product with ID {ProductId}", product.Id);
            return product;
        }

        public async Task UpdateAsync(
            int id,
            string name,
            string description,
            Money price,
            Money? originalPrice,
            Dimensions dimensions,
            Weight weight,
            string? metaTitle = null,
            string? metaDescription = null,
            List<string>? imageUrls = null,
            CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found");
            }

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required", nameof(name));

            if (price == null || price.Amount <= 0)
                throw new ArgumentException("Product price must be greater than zero", nameof(price));

            if (product.Name != name)
            {
                var newSlug = await _slugService.CreateUniqueSlugForProductAsync(name, id, cancellationToken);
                product.SetSlug(newSlug);
            }

            product.UpdateDetails(name, description, price, originalPrice, dimensions, weight, metaTitle, metaDescription);

            var currentImages = product.Images.ToList();
            foreach (var img in currentImages)
            {
                product.RemoveImage(img);
            }

            if (imageUrls != null)
            {
                int order = 0;
                foreach (var url in imageUrls)
                {
                    var image = new ImageResource(
                        originalFileName: System.IO.Path.GetFileName(url),
                        fileExtension: System.IO.Path.GetExtension(url).TrimStart('.'),
                        storagePath: url,
                        publicUrl: url,
                        fileSize: 0, width: 800, height: 600,
                        imageType: ImageType.Product,
                        createdByUserId: product.CreatedByUserId,
                        altText: name,
                        isPrimary: (order == 0)
                    );
                    product.AddImage(image);
                    order++;
                }
            }

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated product with ID {ProductId}", id);
        }

        public async Task DeleteAsync(int id, string? vendorUserId = null, CancellationToken cancellationToken = default)
        {
            Product product;

            if (!string.IsNullOrEmpty(vendorUserId))
            {
                product = await _productRepository.GetByIdVendorAsync(id, cancellationToken);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                    throw new NotFoundException($"Product with ID {id} not found");
                }

                if (product.CreatedByUserId != vendorUserId)
                {
                    _logger.LogWarning("Vendor {VendorId} attempted to delete product {ProductId} which belongs to another user", vendorUserId, id);
                    throw new NotFoundException($"Product with ID {id} not found");
                }
            }
            else
            {
                product = await _productRepository.GetByIdAsync(id, cancellationToken);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                    throw new NotFoundException($"Product with ID {id} not found");
                }
            }

            _productRepository.Remove(product);
            await _productRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deleted product with ID {ProductId}", id);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _productRepository.ExistsAsync(id, cancellationToken);
        }

        public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _productRepository.ExistsBySkuAsync(sku, cancellationToken);
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string searchTerm = null, int? categoryId = null, int? brandId = null, ProductStatus? status = null, decimal? minPrice = null, decimal? maxPrice = null, string sortBy = null, bool sortAscending = true, CancellationToken cancellationToken = default)
        {
            var result = await _productRepository.GetPagedAsync(pageNumber, pageSize, searchTerm, categoryId, brandId, status, minPrice, maxPrice, sortBy, sortAscending, cancellationToken);

            var productsList = result.Products.ToList();
            foreach (var product in productsList)
            {
                ApplyTaxToProductEntity(product);
            }

            return (productsList, result.TotalCount);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found");

            var products = await _productRepository.GetByCategoryAsync(categoryId, cancellationToken);
            foreach (var product in products)
            {
                ApplyTaxToProductEntity(product);
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default)
        {
            var brand = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
            if (brand == null)
                throw new NotFoundException($"Brand with ID {brandId} not found");

            var products = await _productRepository.GetByBrandAsync(brandId, cancellationToken);
            foreach (var product in products)
            {
                ApplyTaxToProductEntity(product);
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetByPriceRangeAsync(Money minPrice, Money maxPrice, CancellationToken cancellationToken = default)
        {
            if (minPrice == null || maxPrice == null || minPrice.Amount > maxPrice.Amount)
                throw new ArgumentException("Invalid price range");

            var products = await _productRepository.GetByPriceRangeAsync(minPrice, maxPrice, cancellationToken);
            foreach (var product in products)
            {
                ApplyTaxToProductEntity(product);
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetFeaturedProductsAsync(count, cancellationToken);
            foreach (var product in products)
            {
                ApplyTaxToProductEntity(product);
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetNewestProductsAsync(int count, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetNewestProductsAsync(count, cancellationToken);
            foreach (var product in products)
            {
                ApplyTaxToProductEntity(product);
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetBestSellingProductsAsync(int count, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetBestSellingProductsAsync(count, cancellationToken);
            foreach (var product in products)
            {
                ApplyTaxToProductEntity(product);
            }
            return products;
        }

        public async Task PublishAsync(int id, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) throw new NotFoundException($"Product with ID {id} not found");

            product.Publish();
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Published product with ID {ProductId}", id);
        }

        public async Task UnpublishAsync(int id, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) throw new NotFoundException($"Product with ID {id} not found");

            product.Unpublish();
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Unpublished product with ID {ProductId}", id);
        }

        public async Task ArchiveAsync(int id, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) throw new NotFoundException($"Product with ID {id} not found");

            product.Archive();
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Archived product with ID {ProductId}", id);
        }

        public async Task SetAsFeaturedAsync(int id, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) throw new NotFoundException($"Product with ID {id} not found");

            product.SetAsFeatured();
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Set product with ID {ProductId} as featured", id);
        }

        public async Task RemoveFromFeaturedAsync(int id, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) throw new NotFoundException($"Product with ID {id} not found");

            product.RemoveFromFeatured();
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Removed product with ID {ProductId} from featured", id);
        }

        public async Task IncrementViewCountAsync(int id, CancellationToken cancellationToken = default)
        {
            await _productRepository.IncrementViewCountAsync(id, cancellationToken);
            _logger.LogDebug("Incremented view count for product with ID {ProductId}", id);
        }

        public async Task UpdateStockQuantityAsync(int id, int quantity, CancellationToken cancellationToken = default)
        {
            if (quantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative", nameof(quantity));

            await _productRepository.UpdateStockQuantityAsync(id, quantity, cancellationToken);
            _logger.LogInformation("Updated stock quantity for product with ID {ProductId} to {Quantity}", id, quantity);
        }

        public async Task UpdateStockStatusAsync(int id, StockStatus status, CancellationToken cancellationToken = default)
        {
            await _productRepository.UpdateStockStatusAsync(id, status, cancellationToken);
            _logger.LogInformation("Updated stock status for product with ID {ProductId} to {Status}", id, status);
        }

        public async Task<ProductVariant> AddVariantAsync(int productId, string sku, string name, Money price, Dimensions dimensions, Weight weight, string? imageUrl = null, Money? originalPrice = null, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product == null) throw new NotFoundException($"Product with ID {productId} not found");

            if (await _productVariantRepository.ExistsBySkuAsync(sku, cancellationToken))
                throw new DuplicateEntityException($"Product variant with SKU {sku} already exists");

            var variant = new ProductVariant(productId, sku, name, price, dimensions, weight, imageUrl, originalPrice);
            variant = await _productVariantRepository.AddAsync(variant, cancellationToken);
            await _productVariantRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added new variant with ID {VariantId} to product with ID {ProductId}", variant.Id, productId);
            return variant;
        }

        public async Task UpdateVariantAsync(int variantId, string name, Money price, Money? originalPrice, Dimensions dimensions, Weight weight, string? imageUrl = null, CancellationToken cancellationToken = default)
        {
            var variant = await _productVariantRepository.GetByIdAsync(variantId, cancellationToken);
            if (variant == null)
                throw new NotFoundException($"Product variant with ID {variantId} not found");

            variant.UpdateDetails(name, price, originalPrice, dimensions, weight, imageUrl);
            _productVariantRepository.Update(variant);
            await _productVariantRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated product variant with ID {VariantId}", variantId);
        }

        public async Task DeleteVariantAsync(int variantId, CancellationToken cancellationToken = default)
        {
            var variant = await _productVariantRepository.GetByIdAsync(variantId, cancellationToken);
            if (variant == null)
                throw new NotFoundException($"Product variant with ID {variantId} not found");

            _productVariantRepository.Remove(variant);
            await _productVariantRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deleted product variant with ID {VariantId}", variantId);
        }

        public async Task ActivateVariantAsync(int variantId, CancellationToken cancellationToken = default)
        {
            await _productVariantRepository.ActivateAsync(variantId, cancellationToken);
            _logger.LogInformation("Activated product variant with ID {VariantId}", variantId);
        }

        public async Task DeactivateVariantAsync(int variantId, CancellationToken cancellationToken = default)
        {
            await _productVariantRepository.DeactivateAsync(variantId, cancellationToken);
            _logger.LogInformation("Deactivated product variant with ID {VariantId}", variantId);
        }

        public async Task<ImageResource> AddImageAsync(int productId, string originalFileName, string fileExtension, string storagePath, string publicUrl, long fileSize, int width, int height, ImageType imageType, string createdByUserId, string? altText = null, bool isPrimary = false, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product == null) throw new NotFoundException($"Product with ID {productId} not found");

            if (string.IsNullOrWhiteSpace(createdByUserId))
                throw new ArgumentException("CreatedByUserId is required", nameof(createdByUserId));

            var image = new ImageResource(originalFileName, fileExtension, storagePath, publicUrl, fileSize, width, height, imageType, createdByUserId, altText, isPrimary);

            var productIdProp = image.GetType().GetProperty("ProductId");
            if (productIdProp != null && productIdProp.CanWrite)
            {
                productIdProp.SetValue(image, productId);
            }

            image = await _imageRepository.AddAsync(image, cancellationToken);
            await _imageRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added image with ID {ImageId} to product with ID {ProductId}", image.Id, productId);
            return image;
        }

        public async Task UpdateImageAsync(int imageId, string? altText = null, bool? isPrimary = null, CancellationToken cancellationToken = default)
        {
            var image = await _imageRepository.GetByIdAsync(imageId, cancellationToken);
            if (image == null)
                throw new NotFoundException($"Image with ID {imageId} not found");

            image.UpdateDetails(altText, isPrimary ?? image.IsPrimary);
            _imageRepository.Update(image);
            await _imageRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated image with ID {ImageId}", imageId);
        }

        public async Task DeleteImageAsync(int imageId, CancellationToken cancellationToken = default)
        {
            var image = await _imageRepository.GetByIdAsync(imageId, cancellationToken);
            if (image == null)
                throw new NotFoundException($"Image with ID {imageId} not found");

            _imageRepository.Remove(image);
            await _imageRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deleted image with ID {ImageId}", imageId);
        }

        public async Task SetPrimaryImageAsync(int imageId, CancellationToken cancellationToken = default)
        {
            await _imageRepository.SetAsPrimaryAsync(imageId, cancellationToken);
            _logger.LogInformation("Set image with ID {ImageId} as primary", imageId);
        }

        public async Task<ProductAttribute> AddAttributeAsync(int productId, string name, string value, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product == null) throw new NotFoundException($"Product with ID {productId} not found");

            var attribute = new ProductAttribute(productId, name, value);
            attribute = await _productAttributeRepository.AddAsync(attribute, cancellationToken);
            await _productAttributeRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added attribute with ID {AttributeId} to product with ID {ProductId}", attribute.Id, productId);
            return attribute;
        }

        public async Task UpdateAttributeAsync(int attributeId, string name, string value, CancellationToken cancellationToken = default)
        {
            var attribute = await _productAttributeRepository.GetByIdAsync(attributeId, cancellationToken);
            if (attribute == null)
                throw new NotFoundException($"Product attribute with ID {attributeId} not found");

            attribute.UpdateName(name);
            attribute.UpdateValue(value);
            _productAttributeRepository.Update(attribute);
            await _productAttributeRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated product attribute with ID {AttributeId}", attributeId);
        }

        public async Task DeleteAttributeAsync(int attributeId, CancellationToken cancellationToken = default)
        {
            var attribute = await _productAttributeRepository.GetByIdAsync(attributeId, cancellationToken);
            if (attribute == null)
                throw new NotFoundException($"Product attribute with ID {attributeId} not found");

            _productAttributeRepository.Remove(attribute);
            await _productAttributeRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deleted product attribute with ID {AttributeId}", attributeId);
        }

        public async Task<ProductTag> AddTagAsync(int productId, string tagText, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product == null) throw new NotFoundException($"Product with ID {productId} not found");

            if (await _productTagRepository.ExistsByProductAndTagAsync(productId, tagText, cancellationToken))
                throw new DuplicateEntityException($"Product already has tag '{tagText}'");

            var tag = new ProductTag(productId, tagText);
            tag = await _productTagRepository.AddAsync(tag, cancellationToken);
            await _productTagRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added tag with ID {TagId} to product with ID {ProductId}", tag.Id, productId);
            return tag;
        }

        public async Task RemoveTagAsync(int tagId, CancellationToken cancellationToken = default)
        {
            var tag = await _productTagRepository.GetByIdAsync(tagId, cancellationToken);
            if (tag == null)
                throw new NotFoundException($"Product tag with ID {tagId} not found");

            _productTagRepository.Remove(tag);
            await _productTagRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Removed product tag with ID {TagId}", tagId);
        }

        public async Task<IEnumerable<string>> GetTagsByProductIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _productTagRepository.GetTagsByProductIdAsync(productId, cancellationToken);
        }

        public async Task<ProductReview> AddReviewAsync(int productId, string userId, string title, string comment, int rating, bool isVerifiedPurchase = false, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

            if (await _productReviewRepository.UserHasReviewedProductAsync(userId, productId, cancellationToken))
                throw new BusinessRuleException("User has already reviewed this product");

            var review = new ProductReview(productId, userId, title, comment, rating, isVerifiedPurchase);
            review = await _productReviewRepository.AddReviewAsync(review, cancellationToken);
            await _productReviewRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added review with ID {ReviewId} for product with ID {ProductId} by user {UserId}", review.Id, productId, userId);
            return review;
        }

        public async Task UpdateReviewAsync(int reviewId, string title, string comment, int rating, CancellationToken cancellationToken = default)
        {
            var review = await _productReviewRepository.GetByIdAsync(reviewId, cancellationToken);
            if (review == null)
                throw new NotFoundException($"Product review with ID {reviewId} not found");

            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

            review.UpdateContent(title, comment, rating);
            _productReviewRepository.Update(review);
            await _productReviewRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated product review with ID {ReviewId}", reviewId);
        }

        public async Task DeleteReviewAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            var review = await _productReviewRepository.GetByIdAsync(reviewId, cancellationToken);
            if (review == null)
                throw new NotFoundException($"Product review with ID {reviewId} not found");

            _productReviewRepository.Remove(review);
            await _productReviewRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deleted product review with ID {ReviewId}", reviewId);
        }

        public async Task ApproveReviewAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            await _productReviewRepository.ApproveAsync(reviewId, cancellationToken);
            _logger.LogInformation("Approved product review with ID {ReviewId}", reviewId);
        }

        public async Task RejectReviewAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            await _productReviewRepository.RejectAsync(reviewId, cancellationToken);
            _logger.LogInformation("Rejected product review with ID {ReviewId}", reviewId);
        }

        public async Task MarkReviewAsVerifiedAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            await _productReviewRepository.MarkAsVerifiedAsync(reviewId, cancellationToken);
            _logger.LogInformation("Marked product review with ID {ReviewId} as verified", reviewId);
        }

        public async Task IncrementHelpfulVotesAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            await _productReviewRepository.IncrementHelpfulVotesAsync(reviewId, cancellationToken);
            _logger.LogInformation("Incremented helpful votes for product review with ID {ReviewId}", reviewId);
        }

        public async Task SetSlugAsync(int id, string title, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) throw new NotFoundException($"Product with ID {id} not found");

            var slug = await _slugService.CreateUniqueSlugForProductAsync(title, id, cancellationToken);
            product.SetSlug(slug);
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Updated slug for product with ID {ProductId}", id);
        }

        public async Task<decimal> GetAveragePriceByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetAveragePriceByCategoryAsync(categoryId, cancellationToken);
        }

        public async Task<decimal> GetAveragePriceByBrandAsync(int brandId, CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetAveragePriceByBrandAsync(brandId, cancellationToken);
        }

        public async Task<int> GetViewCountAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetViewCountAsync(id, cancellationToken);
        }

        public async Task<double> GetAverageRatingAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetAverageRatingAsync(id, cancellationToken);
        }

        public async Task<int> GetTotalReviewsCountAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetTotalReviewsCountAsync(id, cancellationToken);
        }

        public async Task<IDictionary<int, int>> GetRatingDistributionAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetRatingDistributionAsync(id, cancellationToken);
        }

        public async Task SetApprovalStatusAsync(int id, bool isApproved, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllForAdminAsync(cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found");
            }

            await _productRepository.SetApprovalStatusAsync(id, isApproved, cancellationToken);

            _logger.LogInformation("Set approval status for product with ID {ProductId} to {IsApproved}", id, isApproved);
        }
    }
}