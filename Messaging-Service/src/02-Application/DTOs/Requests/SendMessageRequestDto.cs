namespace Messaging_Service.src._02_Application.DTOs.Requests
{
    public class SendMessageRequestDto
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public List<MessageAttachmentDto> Attachments { get; set; }
    }
    public class MessageAttachmentDto
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string MimeType { get; set; }
    }
}
