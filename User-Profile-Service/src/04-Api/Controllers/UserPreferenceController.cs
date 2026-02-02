using Microsoft.AspNetCore.Mvc;
using User_Profile_Service.src._02_Application.DTOs.Responses;
using User_Profile_Service.src._02_Application.Services.Interfaces;

namespace User_Profile_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/profiles/{profileId}/[controller]")]
    public class UserPreferenceController : ControllerBase
    {
        private readonly IUserPreferenceApplicationService _preferenceService;

        public UserPreferenceController(IUserPreferenceApplicationService preferenceService)
        {
            _preferenceService = preferenceService;
        }

        [HttpGet]
        public async Task<ActionResult<UserPreferenceResponseDto>> GetPreferences(Guid profileId)
        {
            var result = await _preferenceService.GetPreferencesAsync(profileId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePreferences(
            Guid profileId,
            [FromBody] UserPreferenceResponseDto request)
        {
            await _preferenceService.UpdatePreferencesAsync(
                profileId,
                request.LanguageCode,
                request.CurrencyCode,
                request.NotificationsEnabled,
                request.Theme);

            return NoContent();
        }
    }
}
