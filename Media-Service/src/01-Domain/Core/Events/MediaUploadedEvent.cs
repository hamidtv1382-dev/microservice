namespace Media_Service.src._01_Domain.Core.Events
{
    public class MediaUploadedEvent
    {
        public Guid MediaFileId { get; set; }
        public string FullPath { get; set; }
        public DateTime OccurredOn { get; set; }

        public MediaUploadedEvent(Guid mediaFileId, string fullPath)
        {
            MediaFileId = mediaFileId;
            FullPath = fullPath;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
