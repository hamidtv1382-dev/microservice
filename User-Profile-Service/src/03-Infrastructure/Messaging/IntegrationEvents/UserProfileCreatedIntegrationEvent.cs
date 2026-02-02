namespace User_Profile_Service.src._03_Infrastructure.Messaging.IntegrationEvents
{
    public class UserProfileCreatedIntegrationEvent
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public DateTime OccurredOn { get; set; }

        public UserProfileCreatedIntegrationEvent(Guid profileId, Guid userId, string email)
        {
            ProfileId = profileId;
            UserId = userId;
            Email = email;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
