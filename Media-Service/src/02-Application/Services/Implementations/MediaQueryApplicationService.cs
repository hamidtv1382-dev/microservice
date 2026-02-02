using Media_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Media_Service.src._01_Domain.Services.Interfaces;
using Media_Service.src._02_Application.DTOs.Requests;
using Media_Service.src._02_Application.DTOs.Responses;
using Media_Service.src._02_Application.Exceptions;
using Media_Service.src._02_Application.Services.Interfaces;
using Media_Service.src._03._Infrastructure.Storage;

namespace Media_Service.src._02_Application.Services.Implementations
{
    public class MediaQueryApplicationService : IMediaQueryApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaClassificationService _classificationService;
        private readonly IFileStorageService _storageService;

        public MediaQueryApplicationService(IUnitOfWork unitOfWork, IMediaClassificationService classificationService, IFileStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _classificationService = classificationService;
            _storageService = storageService;
        }

        public async Task<MediaPathResponseDto> ResolvePathAsync(ResolveMediaPathRequestDto request)
        {
            var relativePath = await _classificationService.ResolvePathAsync(request.OwnerType, request.OwnerId, request.CategoryId, request.SubCategoryId);
            var absoluteUrl = await _storageService.GetAbsoluteUrlAsync(relativePath);

            return new MediaPathResponseDto
            {
                RelativePath = relativePath,
                AbsoluteUrl = absoluteUrl
            };
        }

        public async Task<MediaUploadResponseDto> GetMediaAsync(Guid fileId)
        {
            var file = await _unitOfWork.MediaFiles.GetByIdAsync(fileId);
            if (file == null) throw new MediaOwnerNotFoundException("Media file not found.");

            return new MediaUploadResponseDto
            {
                MediaFileId = file.Id,
                FileName = file.OriginalFileName,
                FullPath = file.RelativePath,
                AbsoluteUrl = file.AbsoluteUrl,
                Type = file.Type,
                UploadedAt = file.CreatedAt
            };
        }
    }
}
