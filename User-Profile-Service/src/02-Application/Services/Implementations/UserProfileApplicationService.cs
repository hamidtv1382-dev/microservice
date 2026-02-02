using AutoMapper;
using User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile;
using User_Profile_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using User_Profile_Service.src._01_Domain.Core.ValueObjects;
using User_Profile_Service.src._02_Application.DTOs.Requests;
using User_Profile_Service.src._02_Application.DTOs.Responses;
using User_Profile_Service.src._02_Application.Exceptions;
using User_Profile_Service.src._02_Application.Interfaces;
using User_Profile_Service.src._02_Application.Services.Interfaces;

namespace User_Profile_Service.src._02_Application.Services.Implementations
{
    public class UserProfileApplicationService : IUserProfileApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;

        public UserProfileApplicationService(IUnitOfWork unitOfWork, IIdentityService identityService, IMediaService mediaService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _mediaService = mediaService;
            _mapper = mapper;
        }

        public async Task<UserProfileResponseDto> CreateProfileAsync(CreateUserProfileRequestDto request)
        {
            // Validate User Exists in Identity Service
            var userExists = await _identityService.ValidateUserExistsAsync(request.UserId);
            if (!userExists)
            {
                throw new InvalidProfileOperationException($"User with ID {request.UserId} does not exist.");
            }

            if (await _unitOfWork.UserProfiles.ExistsAsync(request.UserId))
            {
                throw new InvalidProfileOperationException($"Profile already exists for user {request.UserId}.");
            }

            var name = new FullName(request.FirstName, request.LastName);
            var email = new EmailAddress(request.Email);
            PhoneNumber? phone = null;
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                phone = new PhoneNumber(request.PhoneNumber, request.CountryCode);
            }

            BirthDate? birthDate = null;
            if (request.BirthDate.HasValue)
            {
                birthDate = new BirthDate(request.BirthDate.Value);
            }

            var contactInfo = new UserContactInfo(email, phone);
            var preferences = new UserPreference("en", "USD", true, "Light");

            var profile = new UserProfile(request.UserId, name, contactInfo, preferences);

            if (birthDate != null) profile.UpdateBirthDate(birthDate);

            await _unitOfWork.UserProfiles.AddAsync(profile);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserProfileResponseDto>(profile);
        }

        public async Task<UserProfileResponseDto> GetProfileAsync(Guid profileId)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(profileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {profileId} not found.");
            }

            return _mapper.Map<UserProfileResponseDto>(profile);
        }

        public async Task<UserProfileResponseDto> GetProfileByUserIdAsync(Guid userId)
        {
            var profile = await _unitOfWork.UserProfiles.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile for User ID {userId} not found.");
            }

            return _mapper.Map<UserProfileResponseDto>(profile);
        }

        public async Task<UserProfileResponseDto> UpdateProfileAsync(UpdateUserProfileRequestDto request)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(request.ProfileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {request.ProfileId} not found.");
            }

            var name = new FullName(request.FirstName, request.LastName);
            var email = new EmailAddress(request.Email);

            PhoneNumber? phone = null;
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                phone = new PhoneNumber(request.PhoneNumber, request.CountryCode);
            }

            BirthDate? birthDate = null;
            if (request.BirthDate.HasValue)
            {
                birthDate = new BirthDate(request.BirthDate.Value);
            }

            var contactInfo = new UserContactInfo(email, phone);

            profile.UpdateName(name);
            profile.UpdateContactInfo(contactInfo);

            if (birthDate != null)
            {
                profile.UpdateBirthDate(birthDate);
            }

            _unitOfWork.UserProfiles.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserProfileResponseDto>(profile);
        }

        public async Task DeleteProfileAsync(Guid profileId)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(profileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {profileId} not found.");
            }

            // Soft delete via Domain/Repository logic
            await _unitOfWork.UserProfiles.DeleteAsync(profile);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
