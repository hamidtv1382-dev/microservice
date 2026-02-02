using AutoMapper;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;
using Review_Rating_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Review_Rating_Service.src._01_Domain.Core.ValueObjects;
using Review_Rating_Service.src._02_Application.DTOs.Requests;
using Review_Rating_Service.src._02_Application.DTOs.Responses;
using Review_Rating_Service.src._02_Application.Exceptions;
using Review_Rating_Service.src._02_Application.Services.Interfaces;

namespace Review_Rating_Service.src._02_Application.Services.Implementations
{
    public class RatingApplicationService : IRatingApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RatingResponseDto> AddRatingAsync(AddRatingRequestDto request)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(request.ReviewId);
            if (review == null) throw new ReviewNotFoundException($"Review with ID {request.ReviewId} not found.");

            // Validate rating value via VO
            var ratingValue = new RatingValue(request.Value);

            var rating = new Rating(request.Type, ratingValue);
            review.AddRating(rating);

            _unitOfWork.Reviews.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RatingResponseDto>(rating);
        }
    }
}
