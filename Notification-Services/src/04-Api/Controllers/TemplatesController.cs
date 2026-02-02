using Microsoft.AspNetCore.Mvc;
using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._02_Application.DTOs.Requests;
using Notification_Services.src._02_Application.DTOs.Responses;
using Notification_Services.src._02_Application.Services.Interfaces;

namespace Notification_Services.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplateApplicationService _templateService;

        public TemplatesController(ITemplateApplicationService templateService)
        {
            _templateService = templateService;
        }

        [HttpPost]
        public async Task<ActionResult<TemplateResponseDto>> CreateTemplate([FromBody] CreateTemplateRequestDto request)
        {
            var result = await _templateService.CreateTemplateAsync(request);
            return CreatedAtAction(nameof(GetTemplate), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TemplateResponseDto>> GetTemplate(Guid id)
        {
            var result = await _templateService.GetTemplateAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemplateResponseDto>>> GetTemplatesByType([FromQuery] NotificationType type)
        {
            var result = await _templateService.GetTemplatesByTypeAsync(type);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TemplateResponseDto>> UpdateTemplate(Guid id, [FromBody] UpdateTemplateRequestDto request)
        {
            if (id != request.TemplateId)
            {
                return BadRequest("ID mismatch.");
            }

            var result = await _templateService.UpdateTemplateAsync(request);
            return Ok(result);
        }
    }
}
