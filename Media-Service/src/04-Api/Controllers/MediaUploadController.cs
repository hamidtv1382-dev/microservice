using Media_Service.src._02_Application.DTOs.Requests;
using Media_Service.src._02_Application.DTOs.Responses;
using Media_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Media_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaUploadController : ControllerBase
    {
        private readonly IMediaUploadApplicationService _uploadService;

        public MediaUploadController(IMediaUploadApplicationService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("brand")]
        public async Task<ActionResult<MediaUploadResponseDto>> UploadBrandMedia([FromBody] UploadBrandMediaRequestDto request)
        {
            var result = await _uploadService.UploadBrandMediaAsync(request);
            return CreatedAtAction(nameof(MediaQueryController.GetMedia), new { id = result.MediaFileId }, result);
        }

        [HttpPost("category")]
        public async Task<ActionResult<MediaUploadResponseDto>> UploadCategoryMedia([FromBody] UploadCategoryMediaRequestDto request)
        {
            var result = await _uploadService.UploadCategoryMediaAsync(request);
            return CreatedAtAction(nameof(MediaQueryController.GetMedia), new { id = result.MediaFileId }, result);
        }

        [HttpPost("product")]
        public async Task<ActionResult<MediaUploadResponseDto>> UploadProductMedia([FromBody] UploadProductMediaRequestDto request)
        {
            var result = await _uploadService.UploadProductMediaAsync(request);
            return CreatedAtAction(nameof(MediaQueryController.GetMedia), new { id = result.MediaFileId }, result);
        }
    }
}

