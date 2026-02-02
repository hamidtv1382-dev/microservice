using Microsoft.EntityFrameworkCore;
using User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile;
using User_Profile_Service.src._01_Domain.Core.Interfaces.Repositories;
using User_Profile_Service.src._03_Infrastructure.Data;

namespace User_Profile_Service.src._03_Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetByIdAsync(Guid id)
        {
            return await _context.UserProfiles
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserProfiles
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);
        }

        public async Task AddAsync(UserProfile profile)
        {
            await _context.UserProfiles.AddAsync(profile);
        }

        public async Task UpdateAsync(UserProfile profile)
        {
            _context.UserProfiles.Update(profile);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(UserProfile profile)
        {
            profile.MarkAsDeleted();
            _context.UserProfiles.Update(profile);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _context.UserProfiles
                .AnyAsync(u => u.UserId == userId && !u.IsDeleted);
        }
    }
}
