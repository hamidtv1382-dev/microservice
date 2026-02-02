using User_Profile_Service.src._02_Application.DTOs.Requests;
using User_Profile_Service.src._02_Application.DTOs.Responses;

namespace User_Profile_Service.src._02_Application.Services.Interfaces
{
    public interface IUserProfileApplicationService
    {
        Task<UserProfileResponseDto> CreateProfileAsync(CreateUserProfileRequestDto request);
        Task<UserProfileResponseDto> GetProfileAsync(Guid profileId);
        Task<UserProfileResponseDto> GetProfileByUserIdAsync(Guid userId);
        Task<UserProfileResponseDto> UpdateProfileAsync(UpdateUserProfileRequestDto request);
        Task DeleteProfileAsync(Guid profileId);
    }
}
