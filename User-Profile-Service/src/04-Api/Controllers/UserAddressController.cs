using Microsoft.AspNetCore.Mvc;
using User_Profile_Service.src._02_Application.DTOs.Requests;
using User_Profile_Service.src._02_Application.DTOs.Responses;
using User_Profile_Service.src._02_Application.Services.Interfaces;

namespace User_Profile_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/profiles/{profileId}/[controller]")]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressApplicationService _addressService;

        public UserAddressController(IUserAddressApplicationService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost]
        public async Task<ActionResult<UserAddressResponseDto>> AddAddress(Guid profileId, [FromBody] AddUserAddressRequestDto request)
        {
            if (profileId != request.ProfileId)
            {
                return BadRequest("Profile ID mismatch.");
            }

            var result = await _addressService.AddAddressAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserAddressResponseDto>>> GetAddresses(Guid profileId)
        {
            var result = await _addressService.GetAddressesAsync(profileId);
            return Ok(result);
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> RemoveAddress(Guid profileId, Guid addressId)
        {
            await _addressService.RemoveAddressAsync(profileId, addressId);
            return NoContent();
        }

        [HttpPut("{addressId}/default")]
        public async Task<IActionResult> SetDefaultAddress(Guid profileId, Guid addressId)
        {
            await _addressService.SetDefaultAddressAsync(profileId, addressId);
            return Ok();
        }
    }
}
