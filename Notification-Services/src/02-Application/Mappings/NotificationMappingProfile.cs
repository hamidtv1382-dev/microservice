using AutoMapper;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;
using Notification_Services.src._02_Application.DTOs.Responses;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Notification_Services.src._02_Application.Mappings
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<Notification, NotificationResponseDto>();
            CreateMap<NotificationRecipient, NotificationRecipientDto>();
            CreateMap<NotificationAttachment, NotificationAttachmentDto>();
            CreateMap<NotificationTemplate, TemplateResponseDto>();
        }
    }
}
