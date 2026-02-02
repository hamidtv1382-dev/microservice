using AutoMapper;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;
using Review_Rating_Service.src._01_Domain.Core.Enums;
using Review_Rating_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Review_Rating_Service.src._01_Domain.Core.ValueObjects;
using Review_Rating_Service.src._01_Domain.Services.Interfaces;
using Review_Rating_Service.src._02_Application.DTOs.Requests;
using Review_Rating_Service.src._02_Application.DTOs.Responses;
using Review_Rating_Service.src._02_Application.Exceptions;
using Review_Rating_Service.src._02_Application.Interfaces;
using Review_Rating_Service.src._02_Application.Services.Interfaces;

namespace Review_Rating_Service.src._02_Application.Services.Implementations
{
    public class ReviewApplicationService : IReviewApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewDomainService _reviewDomainService;
        private readonly IMapper _mapper;
        private readonly IExternalNotificationService _notificationService;

        public ReviewApplicationService(IUnitOfWork unitOfWork, IReviewDomainService reviewDomainService, IMapper mapper, IExternalNotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _reviewDomainService = reviewDomainService;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ReviewResponseDto> CreateReviewAsync(CreateReviewRequestDto request, Guid userId)
        {
            if (!await _reviewDomainService.CanUserReviewAsync(userId, request.ProductId))
                throw new ReviewCreationFailedException("User has already reviewed this product.");

            var review = new Review(request.ProductId, userId, new ReviewerName(request.ReviewerName), new ReviewText(request.Text));

            // Ratings
            if (request.Ratings != null)
            {
                foreach (var r in request.Ratings)
                {
                    var rating = new Rating(r.Type, new RatingValue(r.Value))
                    {
                        Review = review
                    };
                    review.AddRating(rating);
                }
            }

            // Attachments
            if (request.Attachments != null)
            {
                foreach (var a in request.Attachments)
                {
                    var attachment = new ReviewAttachment(a.Url, a.Type)
                    {
                        Review = review
                    };
                    review.AddAttachment(attachment);
                }
            }

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.NotifySellerOnReviewCreatedAsync(review.ProductId, review.Text);

            return _mapper.Map<ReviewResponseDto>(review);
        }


        public async Task<ReviewResponseDto> UpdateReviewAsync(UpdateReviewRequestDto request)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(request.ReviewId);
            if (review == null) throw new ReviewNotFoundException($"Review with ID {request.ReviewId} not found.");

            var newText = new ReviewText(request.Text);
            review.UpdateText(newText);

            _unitOfWork.Reviews.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReviewResponseDto>(review);
        }

        public async Task DeleteReviewAsync(Guid reviewId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
            if (review == null) throw new ReviewNotFoundException($"Review with ID {reviewId} not found.");

            await _unitOfWork.Reviews.DeleteAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedResult<ReviewResponseDto>> GetReviewsAsync(GetReviewsFilterRequestDto filter)
        {
            // This logic is usually in Repository, simplified here for brevity
            // In a real app, use a Specification pattern or flexible Repository query

            IEnumerable<Review> query = Enumerable.Empty<Review>();

            if (filter.ProductId.HasValue)
            {
                query = await _unitOfWork.Reviews.GetByProductIdAsync(filter.ProductId.Value);
            }
            else if (filter.UserId.HasValue)
            {
                query = await _unitOfWork.Reviews.GetByUserIdAsync(filter.UserId.Value);
            }
            else
            {
                // Fallback or Exception if filtering is mandatory
                throw new ArgumentException("At least ProductId or UserId must be provided.");
            }

            // Apply filters in memory (for demo) - move to SQL in production
            if (filter.Status.HasValue)
            {
                query = query.Where(r => r.Status == filter.Status.Value);
            }
            if (filter.MinRating.HasValue)
            {
                query = query.Where(r => r.Ratings.Any(rt => rt.Value >= filter.MinRating.Value));
            }

            var totalCount = query.Count();
            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PagedResult<ReviewResponseDto>
            {
                Items = _mapper.Map<IEnumerable<ReviewResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<ReviewSummaryResponseDto> GetProductSummaryAsync(int productId)
        {
            var reviews = (await _unitOfWork.Reviews.GetByProductIdAsync(productId)).Where(r => r.Status == ReviewStatus.Approved).ToList();

            if (!reviews.Any())
            {
                return new ReviewSummaryResponseDto
                {
                    ProductId = productId,
                    AverageRating = 0,
                    TotalReviews = 0,
                    RatingDistribution = new Dictionary<int, int>()
                };
            }

            var allRatings = reviews.SelectMany(r => r.Ratings).ToList();
            var avg = allRatings.Average(r => r.Value);
            var distribution = allRatings
                .GroupBy(r => r.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            return new ReviewSummaryResponseDto
            {
                ProductId = productId,
                AverageRating = avg,
                TotalReviews = reviews.Count,
                RatingDistribution = distribution
            };
        }

        public async Task ApproveReviewAsync(Guid reviewId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
            if (review == null) throw new ReviewNotFoundException($"Review with ID {reviewId} not found.");

            review.Approve();
            _unitOfWork.Reviews.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RejectReviewAsync(Guid reviewId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
            if (review == null) throw new ReviewNotFoundException($"Review with ID {reviewId} not found.");

            review.Reject();
            _unitOfWork.Reviews.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
