using AutoMapper;
using User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile;
using User_Profile_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using User_Profile_Service.src._02_Application.DTOs.Responses;
using User_Profile_Service.src._02_Application.Exceptions;
using User_Profile_Service.src._02_Application.Services.Interfaces;

namespace User_Profile_Service.src._02_Application.Services.Implementations
{
    public class UserPreferenceApplicationService : IUserPreferenceApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserPreferenceApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserPreferenceResponseDto> GetPreferencesAsync(Guid profileId)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(profileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {profileId} not found.");
            }

            return _mapper.Map<UserPreferenceResponseDto>(profile.Preferences);
        }

        public async Task UpdatePreferencesAsync(Guid profileId, string languageCode, string currencyCode, bool notificationsEnabled, string theme)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(profileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {profileId} not found.");
            }

            var newPreferences = new UserPreference(languageCode, currencyCode, notificationsEnabled, theme);
            profile.UpdatePreferences(newPreferences);

            await _unitOfWork.UserProfiles.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
