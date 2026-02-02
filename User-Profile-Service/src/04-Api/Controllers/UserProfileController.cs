using Microsoft.AspNetCore.Mvc;
using User_Profile_Service.src._02_Application.DTOs.Requests;
using User_Profile_Service.src._02_Application.DTOs.Responses;
using User_Profile_Service.src._02_Application.Services.Interfaces;

namespace User_Profile_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileApplicationService _profileService;

        public UserProfileController(IUserProfileApplicationService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<ActionResult<UserProfileResponseDto>> CreateProfile([FromBody] CreateUserProfileRequestDto request)
        {
            var result = await _profileService.CreateProfileAsync(request);
            return CreatedAtAction(nameof(GetProfile), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileResponseDto>> GetProfile(Guid id)
        {
            var result = await _profileService.GetProfileAsync(id);
            return Ok(result);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<UserProfileResponseDto>> GetProfileByUserId(Guid userId)
        {
            var result = await _profileService.GetProfileByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserProfileResponseDto>> UpdateProfile(Guid id, [FromBody] UpdateUserProfileRequestDto request)
        {
            if (id != request.ProfileId)
            {
                return BadRequest("ID mismatch.");
            }

            var result = await _profileService.UpdateProfileAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            await _profileService.DeleteProfileAsync(id);
            return NoContent();
        }
    }
}
