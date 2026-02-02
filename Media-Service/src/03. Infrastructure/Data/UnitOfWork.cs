using Media_Service.src._01_Domain.Core.Interfaces.Repositories;
using Media_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Media_Service.src._03._Infrastructure.Repositories;

namespace Media_Service.src._03._Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IMediaFileRepository _mediaFileRepository;
        private IMediaFolderRepository _mediaFolderRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IMediaFileRepository MediaFiles
        {
            get
            {
                if (_mediaFileRepository == null)
                    _mediaFileRepository = new MediaFileRepository(_context);
                return _mediaFileRepository;
            }
        }

        public IMediaFolderRepository MediaFolders
        {
            get
            {
                if (_mediaFolderRepository == null)
                    _mediaFolderRepository = new MediaFolderRepository(_context);
                return _mediaFolderRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
