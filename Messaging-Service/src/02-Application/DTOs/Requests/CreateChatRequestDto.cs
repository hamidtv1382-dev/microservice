using Messaging_Service.src._01_Domain.Core.Enums;

namespace Messaging_Service.src._02_Application.DTOs.Requests
{
    public class CreateChatRequestDto
    {
        public string Title { get; set; }
        public ChatType Type { get; set; }
        public Guid CreatorId { get; set; }
        public Guid ParticipantId { get; set; }
        public RecipientType ParticipantRole { get; set; }
        public Guid ReferenceId { get; set; }
    }
}
