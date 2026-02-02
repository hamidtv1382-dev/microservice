using Messaging_Service.src._01_Domain.Core.Interfaces.Services;

namespace Messaging_Service.src._03_Infrastructure.Services.Moderation
{
    public class ContentModerationService : IModerationService
    {
        private readonly string[] _badWords = { "spam", "offensive", "badword" };

        // اصلاح: تغییر نوع پارامتر senderId از int به Guid
        public async Task<bool> ValidateContentAsync(string content, Guid senderId)
        {
            await Task.CompletedTask;
            var lowerContent = content.ToLower();
            return !_badWords.Any(badWord => lowerContent.Contains(badWord));
        }
    }
}
