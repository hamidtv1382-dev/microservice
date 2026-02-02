using Media_Service.src._01_Domain.Core.Entities;
using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._01_Domain.Core.Interfaces.Repositories;
using Media_Service.src._03._Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Media_Service.src._03._Infrastructure.Repositories
{
    public class MediaFileRepository : IMediaFileRepository
    {
        private readonly AppDbContext _context;

        public MediaFileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MediaFile?> GetByIdAsync(Guid id)
        {
            return await _context.MediaFiles.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task<IEnumerable<MediaFile>> GetByOwnerIdAsync(Guid ownerId, MediaOwnerType ownerType)
        {
            return await _context.MediaFiles
                .Where(m => m.OwnerId == ownerId && m.OwnerType == ownerType && !m.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(MediaFile mediaFile)
        {
            await _context.MediaFiles.AddAsync(mediaFile);
        }

        public async Task UpdateAsync(MediaFile mediaFile)
        {
            _context.MediaFiles.Update(mediaFile);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(MediaFile mediaFile)
        {
            mediaFile.MarkAsDeleted();
            _context.MediaFiles.Update(mediaFile);
            await Task.CompletedTask;
        }
    }
}
