using Review_Rating_Service.src._01_Domain.Services.Interfaces;

namespace Review_Rating_Service.src._01_Domain.Services.Implementations
{
    public class RatingDomainService : IRatingDomainService
    {
        public void ValidateRatingValue(int value)
        {
            if (value < 1 || value > 5)
                throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 1 and 5.");
        }
    }
}
