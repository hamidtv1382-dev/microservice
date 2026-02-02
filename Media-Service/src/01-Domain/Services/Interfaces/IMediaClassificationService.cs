using Media_Service.src._01_Domain.Core.Entities;
using Media_Service.src._01_Domain.Core.Enums;

namespace Media_Service.src._01_Domain.Services.Interfaces
{
    public interface IMediaClassificationService
    {
        Task<string> ResolvePathAsync(MediaOwnerType ownerType, Guid ownerId, Guid? categoryId = null, Guid? subCategoryId = null);
        Task<MediaFolder> EnsureFolderStructureAsync(MediaOwnerType ownerType, Guid ownerId, Guid? categoryId = null, Guid? subCategoryId = null);
    }
}
