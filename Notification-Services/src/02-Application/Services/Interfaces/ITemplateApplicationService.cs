using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._02_Application.DTOs.Requests;
using Notification_Services.src._02_Application.DTOs.Responses;

namespace Notification_Services.src._02_Application.Services.Interfaces
{
    public interface ITemplateApplicationService
    {
        Task<TemplateResponseDto> CreateTemplateAsync(CreateTemplateRequestDto request);
        Task<TemplateResponseDto> UpdateTemplateAsync(UpdateTemplateRequestDto request);
        Task<TemplateResponseDto> GetTemplateAsync(Guid id);
        Task<List<TemplateResponseDto>> GetTemplatesByTypeAsync(NotificationType type);
    }
}
