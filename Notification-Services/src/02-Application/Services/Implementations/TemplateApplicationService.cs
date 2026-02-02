using AutoMapper;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;
using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._01_Domain.Core.Interfaces.UnitOfWork;
using Notification_Services.src._02_Application.DTOs.Requests;
using Notification_Services.src._02_Application.DTOs.Responses;
using Notification_Services.src._02_Application.Exceptions;
using Notification_Services.src._02_Application.Services.Interfaces;

namespace Notification_Services.src._02_Application.Services.Implementations
{
    public class TemplateApplicationService : ITemplateApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TemplateApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TemplateResponseDto> CreateTemplateAsync(CreateTemplateRequestDto request)
        {
            var template = new NotificationTemplate(request.Name, request.Subject, request.BodyContent, request.Type, request.LanguageCode);

            await _unitOfWork.Templates.AddAsync(template);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TemplateResponseDto>(template);
        }

        public async Task<TemplateResponseDto> UpdateTemplateAsync(UpdateTemplateRequestDto request)
        {
            var template = await _unitOfWork.Templates.GetByIdAsync(request.TemplateId);
            if (template == null) throw new TemplateNotFoundException("Template not found.");

            template.UpdateContent(request.Subject, request.BodyContent);

            await _unitOfWork.Templates.UpdateAsync(template);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TemplateResponseDto>(template);
        }

        public async Task<TemplateResponseDto> GetTemplateAsync(Guid id)
        {
            var template = await _unitOfWork.Templates.GetByIdAsync(id);
            if (template == null) throw new TemplateNotFoundException("Template not found.");

            return _mapper.Map<TemplateResponseDto>(template);
        }

        public async Task<List<TemplateResponseDto>> GetTemplatesByTypeAsync(NotificationType type)
        {
            var templates = await _unitOfWork.Templates.GetByTypeAsync(type);
            return _mapper.Map<List<TemplateResponseDto>>(templates);
        }
    }
}
