using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._02_Application.DTOs.Responses;

namespace Media_Service.src._02_Application.Services.Interfaces
{
    public interface IMediaFolderApplicationService
    {
        Task<MediaFolderResponseDto> EnsureFolderExistsAsync(Guid ownerId, MediaOwnerType ownerType, Guid? categoryId = null, Guid? subCategoryId = null);
    }
}
