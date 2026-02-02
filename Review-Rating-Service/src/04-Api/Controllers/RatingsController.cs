using Microsoft.AspNetCore.Mvc;
using Review_Rating_Service.src._02_Application.DTOs.Requests;
using Review_Rating_Service.src._02_Application.DTOs.Responses;
using Review_Rating_Service.src._02_Application.Services.Interfaces;

namespace Review_Rating_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingApplicationService _ratingService;

        public RatingsController(IRatingApplicationService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<ActionResult<RatingResponseDto>> AddRating([FromBody] AddRatingRequestDto request)
        {
            var result = await _ratingService.AddRatingAsync(request);
            return Ok(result);
        }
    }
}
