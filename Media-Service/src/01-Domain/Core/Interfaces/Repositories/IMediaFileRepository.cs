using Media_Service.src._01_Domain.Core.Entities;
using Media_Service.src._01_Domain.Core.Enums;

namespace Media_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IMediaFileRepository
    {
        Task<MediaFile?> GetByIdAsync(Guid id);
        Task<IEnumerable<MediaFile>> GetByOwnerIdAsync(Guid ownerId, MediaOwnerType ownerType);
        Task AddAsync(MediaFile mediaFile);
        Task UpdateAsync(MediaFile mediaFile);
        Task DeleteAsync(MediaFile mediaFile);
    }
}
