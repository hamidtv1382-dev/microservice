using User_Profile_Service.src._01_Domain.Core.Interfaces.Repositories;
using User_Profile_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using User_Profile_Service.src._03_Infrastructure.Repositories;

namespace User_Profile_Service.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IUserProfileRepository _userProfileRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserProfileRepository UserProfiles
        {
            get
            {
                if (_userProfileRepository == null)
                    _userProfileRepository = new UserProfileRepository(_context);
                return _userProfileRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
