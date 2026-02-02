using Review_Rating_Service.src._01_Domain.Core.Interfaces.Repositories;
using Review_Rating_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Review_Rating_Service.src._03_Infrastructure.Repositories;

namespace Review_Rating_Service.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IReviewRepository _reviewRepository;
        private IRatingRepository _ratingRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IReviewRepository Reviews
        {
            get
            {
                if (_reviewRepository == null)
                    _reviewRepository = new ReviewRepository(_context);
                return _reviewRepository;
            }
        }

        public IRatingRepository Ratings
        {
            get
            {
                if (_ratingRepository == null)
                    _ratingRepository = new RatingRepository(_context);
                return _ratingRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
