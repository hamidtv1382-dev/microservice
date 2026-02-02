using Messaging_Service.src._01_Domain.Core.Common;

namespace Messaging_Service.src._01_Domain.Core.Entities
{
    public class MessageAttachment : BaseEntity
    {
        public Guid MessageId { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public long FileSize { get; private set; }
        public string MimeType { get; private set; }

        protected MessageAttachment() { }

        public MessageAttachment(Guid messageId, string fileName, string filePath, long fileSize, string mimeType)
        {
            Id = Guid.NewGuid();
            MessageId = messageId;
            FileName = fileName;
            FilePath = filePath;
            FileSize = fileSize;
            MimeType = mimeType;
        }
    }
}
