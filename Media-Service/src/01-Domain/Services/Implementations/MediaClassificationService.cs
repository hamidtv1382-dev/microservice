using Media_Service.src._01_Domain.Core.Entities;
using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Media_Service.src._01_Domain.Services.Interfaces;
using Media_Service.src._02_Application.Interfaces;
using Media_Service.src._03._Infrastructure.Storage;

namespace Media_Service.src._01_Domain.Services.Implementations
{
    public class MediaClassificationService : IMediaClassificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrandCatalogService _brandCatalogService;
        private readonly ICategoryCatalogService _categoryCatalogService;
        private readonly IStoragePathResolver _storagePathResolver;
        private readonly ILoggingService _loggingService;

        public MediaClassificationService(IUnitOfWork unitOfWork, IBrandCatalogService brandCatalogService, ICategoryCatalogService categoryCatalogService, IStoragePathResolver storagePathResolver, ILoggingService loggingService)
        {
            _unitOfWork = unitOfWork;
            _brandCatalogService = brandCatalogService;
            _categoryCatalogService = categoryCatalogService;
            _storagePathResolver = storagePathResolver;
            _loggingService = loggingService;
        }

        public async Task<string> ResolvePathAsync(MediaOwnerType ownerType, Guid ownerId, Guid? categoryId = null, Guid? subCategoryId = null)
        {
            var folder = await EnsureFolderStructureAsync(ownerType, ownerId, categoryId, subCategoryId);
            return folder.FullPhysicalPath;
        }

        public async Task<MediaFolder> EnsureFolderStructureAsync(MediaOwnerType ownerType, Guid ownerId, Guid? categoryId = null, Guid? subCategoryId = null)
        {
            var rootPath = await _storagePathResolver.GetRootPathAsync();
            MediaFolder currentFolder = null;
            Guid? currentOwnerId = null;

            // Logic to build hierarchy based on owner type

            if (ownerType == MediaOwnerType.Brand)
            {
                var brand = await _brandCatalogService.GetBrandByIdAsync(ownerId);
                if (brand == null) throw new Exception("Brand not found");

                var brandFolderName = Slugify(brand.Name);
                currentFolder = await EnsureFolderExists(rootPath, brandFolderName, MediaOwnerType.Brand, ownerId, null);
                currentOwnerId = currentFolder.Id;
            }
            else if (ownerType == MediaOwnerType.Category)
            {
                // Brand -> Category
                // Assuming category belongs to a brand, we might need brandId in input or fetch from category
                var category = await _categoryCatalogService.GetCategoryByIdAsync(categoryId ?? ownerId);

                // Simplified: Just Category under root or Brand if known. 
                // Let's assume we fetch Brand from Category if available
                var brandFolderName = Slugify(category?.BrandName ?? "unknown-brand");
                var brandFolder = await EnsureFolderExists(rootPath, brandFolderName, MediaOwnerType.Brand, category?.BrandId ?? Guid.Empty, null);

                var categoryFolderName = Slugify(category?.Name ?? "unknown-category");
                currentFolder = await EnsureFolderExists(brandFolder.FullPhysicalPath, categoryFolderName, MediaOwnerType.Category, categoryId, brandFolder.Id);
                currentOwnerId = currentFolder.Id;
            }
            else if (ownerType == MediaOwnerType.SubCategory)
            {
                // Brand -> Category -> SubCategory
                var category = await _categoryCatalogService.GetCategoryByIdAsync(categoryId ?? throw new ArgumentNullException(nameof(categoryId)));

                var brandFolderName = Slugify(category.BrandName);
                var brandFolder = await EnsureFolderExists(rootPath, brandFolderName, MediaOwnerType.Brand, category.BrandId ?? Guid.Empty, null);

                var categoryFolderName = Slugify(category.Name);
                var categoryFolder = await EnsureFolderExists(brandFolder.FullPhysicalPath, categoryFolderName, MediaOwnerType.Category, categoryId, brandFolder.Id);

                var subCategory = await _categoryCatalogService.GetCategoryByIdAsync(subCategoryId ?? ownerId); // Assuming SubCategory is also in Category Service
                var subFolderName = Slugify(subCategory?.Name ?? "unknown-sub");
                currentFolder = await EnsureFolderExists(categoryFolder.FullPhysicalPath, subFolderName, MediaOwnerType.SubCategory, subCategoryId, categoryFolder.Id);
                currentOwnerId = currentFolder.Id;
            }
            else if (ownerType == MediaOwnerType.Product)
            {
                // Brand -> Category -> SubCategory -> products
                // Product media goes into 'products' folder
                var category = await _categoryCatalogService.GetCategoryByIdAsync(categoryId ?? throw new ArgumentNullException(nameof(categoryId)));

                var brandFolderName = Slugify(category.BrandName);
                var brandFolder = await EnsureFolderExists(rootPath, brandFolderName, MediaOwnerType.Brand, category.BrandId ?? Guid.Empty, null);

                var categoryFolderName = Slugify(category.Name);
                var categoryFolder = await EnsureFolderExists(brandFolder.FullPhysicalPath, categoryFolderName, MediaOwnerType.Category, categoryId, brandFolder.Id);

                string subFolderName = "products";
                if (subCategoryId.HasValue)
                {
                    var subCategory = await _categoryCatalogService.GetCategoryByIdAsync(subCategoryId.Value);
                    subFolderName = Slugify(subCategory?.Name);
                }

                currentFolder = await EnsureFolderExists(categoryFolder.FullPhysicalPath, subFolderName, MediaOwnerType.SubCategory, subCategoryId, categoryFolder.Id);
                currentOwnerId = currentFolder.Id;
            }

            if (currentFolder == null)
            {
                throw new Exception("Failed to determine media path.");
            }

            return currentFolder;
        }

        private async Task<MediaFolder> EnsureFolderExists(string parentPath, string folderName, MediaOwnerType ownerType, Guid? ownerId, Guid? parentFolderId)
        {
            var fullPath = Path.Combine(parentPath, folderName);

            var existingFolder = await _unitOfWork.MediaFolders.GetByPhysicalPathAsync(fullPath);
            if (existingFolder != null) return existingFolder;

            var newFolder = new MediaFolder(folderName, fullPath, ownerType, ownerId, parentFolderId);
            await _unitOfWork.MediaFolders.AddAsync(newFolder);
            await _unitOfWork.SaveChangesAsync();

            // Simulate physical creation or defer to storage service
            _loggingService.LogInformation($"Folder structure ensured: {fullPath}");

            return newFolder;
        }

        private string Slugify(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "unknown";
            return text.ToLower().Trim().Replace(" ", "-");
        }
    }
}
