using Review_Rating_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Review_Rating_Service.src._01_Domain.Services.Interfaces;

namespace Review_Rating_Service.src._01_Domain.Services.Implementations
{
    public class ReviewDomainService : IReviewDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewDomainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CanUserReviewAsync(Guid userId, int productId)
        {
            var existingReview = await _unitOfWork.Reviews.GetByUserAndProductAsync(userId, productId);
            return existingReview == null;
        }
    }
}
