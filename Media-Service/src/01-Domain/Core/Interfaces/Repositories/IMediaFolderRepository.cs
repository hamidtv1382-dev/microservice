using Media_Service.src._01_Domain.Core.Entities;

namespace Media_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IMediaFolderRepository
    {
        Task<MediaFolder?> GetByPhysicalPathAsync(string path);
        Task<MediaFolder?> GetByIdAsync(Guid id);
        Task<IEnumerable<MediaFolder>> GetByOwnerAsync(Guid ownerId);
        Task AddAsync(MediaFolder folder);
        Task<bool> ExistsAsync(string path);
    }
}
