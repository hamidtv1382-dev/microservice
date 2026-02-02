using User_Profile_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace User_Profile_Service.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserProfileRepository UserProfiles { get; }
        Task<int> SaveChangesAsync();
    }
}
