using AutoMapper;
using User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile;
using User_Profile_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using User_Profile_Service.src._01_Domain.Core.ValueObjects;
using User_Profile_Service.src._02_Application.DTOs.Requests;
using User_Profile_Service.src._02_Application.DTOs.Responses;
using User_Profile_Service.src._02_Application.Exceptions;
using User_Profile_Service.src._02_Application.Services.Interfaces;

namespace User_Profile_Service.src._02_Application.Services.Implementations
{
    public class UserAddressApplicationService : IUserAddressApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAddressApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserAddressResponseDto> AddAddressAsync(AddUserAddressRequestDto request)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(request.ProfileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {request.ProfileId} not found.");
            }

            if (profile.Addresses.Count() >= 5)
            {
                throw new AddressLimitExceededException("User has reached the maximum address limit.");
            }

            var addressValue = new Address(request.Street, request.City, request.State, request.PostalCode, request.Country);
            var address = new UserAddress(Guid.NewGuid(), request.Type, addressValue, request.Title);

            if (request.IsDefault)
            {
                address.SetAsDefault();
            }

            profile.AddAddress(address);

            await _unitOfWork.UserProfiles.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserAddressResponseDto>(address);
        }

        public async Task<List<UserAddressResponseDto>> GetAddressesAsync(Guid profileId)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(profileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {profileId} not found.");
            }

            return _mapper.Map<List<UserAddressResponseDto>>(profile.Addresses.ToList());
        }

        public async Task RemoveAddressAsync(Guid profileId, Guid addressId)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(profileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {profileId} not found.");
            }

            profile.RemoveAddress(addressId);

            await _unitOfWork.UserProfiles.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SetDefaultAddressAsync(Guid profileId, Guid addressId)
        {
            var profile = await _unitOfWork.UserProfiles.GetByIdAsync(profileId);
            if (profile == null)
            {
                throw new UserProfileNotFoundException($"Profile with ID {profileId} not found.");
            }

            var targetAddress = profile.Addresses.FirstOrDefault(a => a.Id == addressId);
            if (targetAddress == null)
            {
                throw new UserProfileNotFoundException($"Address with ID {addressId} not found.");
            }

            profile.SetAddressAsDefault(targetAddress);

            await _unitOfWork.UserProfiles.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
