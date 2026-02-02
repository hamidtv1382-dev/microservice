using Media_Service.src._01_Domain.Core.Entities;
using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Media_Service.src._01_Domain.Core.ValueObjects;
using Media_Service.src._01_Domain.Services.Interfaces;
using Media_Service.src._02_Application.DTOs.Requests;
using Media_Service.src._02_Application.DTOs.Responses;
using Media_Service.src._02_Application.Exceptions;
using Media_Service.src._02_Application.Interfaces;
using Media_Service.src._02_Application.Services.Interfaces;
using Media_Service.src._03._Infrastructure.Storage;

namespace Media_Service.src._02_Application.Services.Implementations
{
    public class MediaUploadApplicationService : IMediaUploadApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaDomainService _domainService;
        private readonly IMediaClassificationService _classificationService;
        private readonly IFileStorageService _storageService;
        private readonly IBrandCatalogService _brandCatalogService;
        private readonly ICategoryCatalogService _categoryCatalogService;

        public MediaUploadApplicationService(IUnitOfWork unitOfWork, IMediaDomainService domainService, IMediaClassificationService classificationService, IFileStorageService storageService, IBrandCatalogService brandCatalogService, ICategoryCatalogService categoryCatalogService)
        {
            _unitOfWork = unitOfWork;
            _domainService = domainService;
            _classificationService = classificationService;
            _storageService = storageService;
            _brandCatalogService = brandCatalogService;
            _categoryCatalogService = categoryCatalogService;
        }

        public async Task<MediaUploadResponseDto> UploadBrandMediaAsync(UploadBrandMediaRequestDto request)
        {
            var bytes = Convert.FromBase64String(request.FileContentBase64);
            _domainService.ValidateFileSize(bytes.Length);
            _domainService.ValidateFileExtension(request.FileName, request.Type);

            // Ensure Path
            var folder = await _classificationService.EnsureFolderStructureAsync(MediaOwnerType.Brand, request.BrandId);

            // Generate Unique Name
            var extension = Path.GetExtension(request.FileName);
            var storedName = $"{Guid.NewGuid()}{extension}";
            var relativePath = Path.Combine(folder.FullPhysicalPath, storedName);

            // Save File
            var absoluteUrl = await _storageService.SaveFileAsync(relativePath, bytes);

            // Create Record
            var fileName = new FileName(request.FileName);
            var fileSize = new FileSize(bytes.Length);
            var fileExt = new FileExtension(extension);
            var audit = new AuditInfo("System");

            var mediaFile = new MediaFile(
                request.FileName,
                storedName,
                extension,
                bytes.Length,
                relativePath,
                absoluteUrl,
                request.Type,
                MediaOwnerType.Brand,
                request.BrandId,
                MediaVisibility.Public,
                audit
            );

            await _unitOfWork.MediaFiles.AddAsync(mediaFile);
            await _unitOfWork.SaveChangesAsync();

            return new MediaUploadResponseDto
            {
                MediaFileId = mediaFile.Id,
                FileName = mediaFile.OriginalFileName,
                FullPath = mediaFile.RelativePath,
                AbsoluteUrl = mediaFile.AbsoluteUrl,
                Type = mediaFile.Type,
                UploadedAt = mediaFile.CreatedAt
            };
        }

        public async Task<MediaUploadResponseDto> UploadCategoryMediaAsync(UploadCategoryMediaRequestDto request)
        {
            var bytes = Convert.FromBase64String(request.FileContentBase64);
            _domainService.ValidateFileSize(bytes.Length);
            _domainService.ValidateFileExtension(request.FileName, request.Type);

            // Verify Category Exists
            var category = await _categoryCatalogService.GetCategoryByIdAsync(request.CategoryId);
            if (category == null) throw new MediaOwnerNotFoundException("Category not found.");

            // Brand -> Category
            var folder = await _classificationService.EnsureFolderStructureAsync(MediaOwnerType.Category, request.CategoryId, category.BrandId);

            var extension = Path.GetExtension(request.FileName);
            var storedName = $"{Guid.NewGuid()}{extension}";
            var relativePath = Path.Combine(folder.FullPhysicalPath, storedName);

            var absoluteUrl = await _storageService.SaveFileAsync(relativePath, bytes);

            var audit = new AuditInfo("System");
            var mediaFile = new MediaFile(
                request.FileName,
                storedName,
                extension,
                bytes.Length,
                relativePath,
                absoluteUrl,
                request.Type,
                MediaOwnerType.Category,
                request.CategoryId,
                MediaVisibility.Public,
                audit
            );

            await _unitOfWork.MediaFiles.AddAsync(mediaFile);
            await _unitOfWork.SaveChangesAsync();

            return new MediaUploadResponseDto
            {
                MediaFileId = mediaFile.Id,
                FileName = mediaFile.OriginalFileName,
                FullPath = mediaFile.RelativePath,
                AbsoluteUrl = mediaFile.AbsoluteUrl,
                Type = mediaFile.Type,
                UploadedAt = mediaFile.CreatedAt
            };
        }

        public async Task<MediaUploadResponseDto> UploadProductMediaAsync(UploadProductMediaRequestDto request)
        {
            var bytes = Convert.FromBase64String(request.FileContentBase64);
            _domainService.ValidateFileSize(bytes.Length);
            _domainService.ValidateFileExtension(request.FileName, request.Type);

            // Product Logic: Brand -> Category -> SubCategory -> products folder
            var folder = await _classificationService.EnsureFolderStructureAsync(MediaOwnerType.Product, request.ProductId, request.CategoryId, request.SubCategoryId);

            var extension = Path.GetExtension(request.FileName);
            var storedName = $"{Guid.NewGuid()}{extension}";
            var relativePath = Path.Combine(folder.FullPhysicalPath, storedName);

            var absoluteUrl = await _storageService.SaveFileAsync(relativePath, bytes);

            var audit = new AuditInfo("System");
            var mediaFile = new MediaFile(
                request.FileName,
                storedName,
                extension,
                bytes.Length,
                relativePath,
                absoluteUrl,
                request.Type,
                MediaOwnerType.Product,
                request.ProductId,
                MediaVisibility.Public,
                audit
            );

            await _unitOfWork.MediaFiles.AddAsync(mediaFile);
            await _unitOfWork.SaveChangesAsync();

            return new MediaUploadResponseDto
            {
                MediaFileId = mediaFile.Id,
                FileName = mediaFile.OriginalFileName,
                FullPath = mediaFile.RelativePath,
                AbsoluteUrl = mediaFile.AbsoluteUrl,
                Type = mediaFile.Type,
                UploadedAt = mediaFile.CreatedAt
            };
        }
    }
}
