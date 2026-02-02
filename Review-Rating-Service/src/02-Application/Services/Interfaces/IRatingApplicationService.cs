using Review_Rating_Service.src._02_Application.DTOs.Requests;
using Review_Rating_Service.src._02_Application.DTOs.Responses;

namespace Review_Rating_Service.src._02_Application.Services.Interfaces
{
    public interface IRatingApplicationService
    {
        Task<RatingResponseDto> AddRatingAsync(AddRatingRequestDto request);
    }
}
