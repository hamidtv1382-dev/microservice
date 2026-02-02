using User_Profile_Service.src._02_Application.DTOs.Responses;

namespace User_Profile_Service.src._02_Application.Services.Interfaces
{
    public interface IUserPreferenceApplicationService
    {
        Task<UserPreferenceResponseDto> GetPreferencesAsync(Guid profileId);
        Task UpdatePreferencesAsync(Guid profileId, string languageCode, string currencyCode, bool notificationsEnabled, string theme);
    }
}
