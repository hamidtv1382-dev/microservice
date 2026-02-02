namespace Messaging_Service.src._01_Domain.Core.Interfaces.Services
{
    public interface IModerationService
    {
        Task<bool> ValidateContentAsync(string content, Guid senderId);
    }
}
