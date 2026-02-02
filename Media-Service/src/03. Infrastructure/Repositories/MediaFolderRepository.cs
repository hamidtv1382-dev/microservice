using Media_Service.src._01_Domain.Core.Entities;
using Media_Service.src._01_Domain.Core.Interfaces.Repositories;
using Media_Service.src._03._Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Media_Service.src._03._Infrastructure.Repositories
{
    public class MediaFolderRepository : IMediaFolderRepository
    {
        private readonly AppDbContext _context;

        public MediaFolderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MediaFolder?> GetByPhysicalPathAsync(string path)
        {
            return await _context.MediaFolders.FirstOrDefaultAsync(m => m.FullPhysicalPath == path && !m.IsDeleted);
        }

        public async Task<MediaFolder?> GetByIdAsync(Guid id)
        {
            return await _context.MediaFolders.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task<IEnumerable<MediaFolder>> GetByOwnerAsync(Guid ownerId)
        {
            return await _context.MediaFolders
                .Where(m => m.OwnerId == ownerId && !m.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(MediaFolder folder)
        {
            await _context.MediaFolders.AddAsync(folder);
        }

        public async Task<bool> ExistsAsync(string path)
        {
            return await _context.MediaFolders.AnyAsync(m => m.FullPhysicalPath == path && !m.IsDeleted);
        }
    }
}
