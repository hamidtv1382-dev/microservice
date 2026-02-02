namespace Media_Service.src._01_Domain.Core.Events.Handlers
{
    public class MediaUploadedEventHandler
    {
        public async Task Handle(MediaUploadedEvent domainEvent)
        {
            // Simulate side-effect: Trigger CDN cache invalidation or Image Processing
            await Task.Delay(100);
            return;
        }
    }
}
