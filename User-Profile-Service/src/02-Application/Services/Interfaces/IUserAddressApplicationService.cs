using User_Profile_Service.src._02_Application.DTOs.Requests;
using User_Profile_Service.src._02_Application.DTOs.Responses;

namespace User_Profile_Service.src._02_Application.Services.Interfaces
{
    public interface IUserAddressApplicationService
    {
        Task<UserAddressResponseDto> AddAddressAsync(AddUserAddressRequestDto request);
        Task<List<UserAddressResponseDto>> GetAddressesAsync(Guid profileId);
        Task RemoveAddressAsync(Guid profileId, Guid addressId);
        Task SetDefaultAddressAsync(Guid profileId, Guid addressId);
    }
}
