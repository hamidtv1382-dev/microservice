namespace Notification_Services.src._01_Domain.Core.Aggregates.Notification
{
    public class NotificationAttachment
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string FileUrl { get; private set; }
        public long FileSizeBytes { get; private set; }

        public NotificationAttachment(string fileName, string fileUrl, long fileSizeBytes)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name is required.", nameof(fileName));
            if (string.IsNullOrWhiteSpace(fileUrl)) throw new ArgumentException("File URL is required.", nameof(fileUrl));
            FileName = fileName;
            FileUrl = fileUrl;
            FileSizeBytes = fileSizeBytes;
        }
    }
}