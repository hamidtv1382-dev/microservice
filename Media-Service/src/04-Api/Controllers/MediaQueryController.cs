using Media_Service.src._02_Application.DTOs.Requests;
using Media_Service.src._02_Application.DTOs.Responses;
using Media_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Media_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaQueryController : ControllerBase
    {
        private readonly IMediaQueryApplicationService _queryService;

        public MediaQueryController(IMediaQueryApplicationService queryService)
        {
            _queryService = queryService;
        }

        [HttpPost("resolve-path")]
        public async Task<ActionResult<MediaPathResponseDto>> ResolvePath([FromBody] ResolveMediaPathRequestDto request)
        {
            var result = await _queryService.ResolvePathAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MediaUploadResponseDto>> GetMedia(Guid id)
        {
            var result = await _queryService.GetMediaAsync(id);
            return Ok(result);
        }
    }
}
