using Notification_Services.src._01_Domain.Core.Aggregates.Notification;
using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._01_Domain.Core.Interfaces.UnitOfWork;
using Notification_Services.src._01_Domain.Services.Interfaces;

namespace Notification_Services.src._01_Domain.Services.Implementations
{
    public class TemplateDomainService : ITemplateDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TemplateDomainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationTemplate?> GetTemplateForTypeAndLanguageAsync(NotificationType type, string languageCode)
        {
            return await _unitOfWork.Templates.GetByTypeAsync(type)
                .ContinueWith(t => t.Result.FirstOrDefault(x => x.LanguageCode == languageCode));
        }
    }
}
