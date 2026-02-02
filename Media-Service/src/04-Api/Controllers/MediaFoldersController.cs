using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._02_Application.DTOs.Responses;
using Media_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Media_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaFoldersController : ControllerBase
    {
        private readonly IMediaFolderApplicationService _folderService;

        public MediaFoldersController(IMediaFolderApplicationService folderService)
        {
            _folderService = folderService;
        }

        [HttpPost("ensure")]
        public async Task<ActionResult<MediaFolderResponseDto>> EnsureFolder([FromQuery] Guid ownerId, MediaOwnerType ownerType, [FromQuery] Guid? categoryId = null, [FromQuery] Guid? subCategoryId = null)
        {
            var result = await _folderService.EnsureFolderExistsAsync(ownerId, ownerType, categoryId, subCategoryId);
            return Ok(result);
        }
    }
}
