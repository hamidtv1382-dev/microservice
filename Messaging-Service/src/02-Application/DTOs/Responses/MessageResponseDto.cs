using Messaging_Service.src._02_Application.DTOs.Requests;

namespace Messaging_Service.src._02_Application.DTOs.Responses
{
    public class MessageResponseDto
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<MessageAttachmentDto> Attachments { get; set; }
    }
}
