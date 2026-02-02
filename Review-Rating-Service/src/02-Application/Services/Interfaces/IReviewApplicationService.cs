using Review_Rating_Service.src._02_Application.DTOs.Requests;
using Review_Rating_Service.src._02_Application.DTOs.Responses;
using Review_Rating_Service.src._02_Application.Services.Implementations;

namespace Review_Rating_Service.src._02_Application.Services.Interfaces
{
    public interface IReviewApplicationService
    {
        Task<ReviewResponseDto> CreateReviewAsync(
            CreateReviewRequestDto request,
            Guid userId);
        Task<ReviewResponseDto> UpdateReviewAsync(UpdateReviewRequestDto request);
        Task DeleteReviewAsync(Guid reviewId);
        Task<PagedResult<ReviewResponseDto>> GetReviewsAsync(GetReviewsFilterRequestDto filter);
        Task<ReviewSummaryResponseDto> GetProductSummaryAsync(int productId);
        Task ApproveReviewAsync(Guid reviewId);
        Task RejectReviewAsync(Guid reviewId);
    }
}
