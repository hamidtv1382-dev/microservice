namespace Media_Service.src._01_Domain.Core.Events
{
    public class MediaFolderCreatedEvent
    {
        public Guid MediaFolderId { get; set; }
        public string FullPath { get; set; }
        public DateTime OccurredOn { get; set; }

        public MediaFolderCreatedEvent(Guid mediaFolderId, string fullPath)
        {
            MediaFolderId = mediaFolderId;
            FullPath = fullPath;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
