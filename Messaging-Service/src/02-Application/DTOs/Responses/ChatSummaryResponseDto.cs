namespace Messaging_Service.src._02_Application.DTOs.Responses
{
    public class ChatSummaryResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Guid> ParticipantIds { get; set; }
    }
}
