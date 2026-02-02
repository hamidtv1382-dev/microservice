using Media_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Media_Service.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IMediaFileRepository MediaFiles { get; }
        IMediaFolderRepository MediaFolders { get; }
        Task<int> SaveChangesAsync();
    }
}
