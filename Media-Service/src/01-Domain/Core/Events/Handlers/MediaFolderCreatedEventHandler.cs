namespace Media_Service.src._01_Domain.Core.Events.Handlers
{
    public class MediaFolderCreatedEventHandler
    {
        public async Task Handle(MediaFolderCreatedEvent domainEvent)
        {
            // Simulate side-effect: Log folder creation event
            await Task.Delay(100);
            return;
        }
    }
}
