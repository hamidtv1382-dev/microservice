using User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile;

namespace User_Profile_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByIdAsync(Guid id);
        Task<UserProfile?> GetByUserIdAsync(Guid userId);
        Task AddAsync(UserProfile profile);
        Task UpdateAsync(UserProfile profile);
        Task DeleteAsync(UserProfile profile);
        Task<bool> ExistsAsync(Guid userId);
    }
}
