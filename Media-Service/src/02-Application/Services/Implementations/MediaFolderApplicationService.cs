using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Media_Service.src._01_Domain.Services.Interfaces;
using Media_Service.src._02_Application.DTOs.Responses;
using Media_Service.src._02_Application.Services.Interfaces;

namespace Media_Service.src._02_Application.Services.Implementations
{
    public class MediaFolderApplicationService : IMediaFolderApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaClassificationService _classificationService;

        public MediaFolderApplicationService(IUnitOfWork unitOfWork, IMediaClassificationService classificationService)
        {
            _unitOfWork = unitOfWork;
            _classificationService = classificationService;
        }

        public async Task<MediaFolderResponseDto> EnsureFolderExistsAsync(Guid ownerId, MediaOwnerType ownerType, Guid? categoryId = null, Guid? subCategoryId = null)
        {
            var folder = await _classificationService.EnsureFolderStructureAsync(ownerType, ownerId, categoryId, subCategoryId);

            return new MediaFolderResponseDto
            {
                FolderId = folder.Id,
                FolderName = folder.FolderName,
                FullPhysicalPath = folder.FullPhysicalPath,
                OwnerType = folder.OwnerType,
                OwnerId = folder.OwnerId
            };
        }
    }
}
