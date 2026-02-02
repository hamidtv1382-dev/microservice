namespace User_Profile_Service.src._03_Infrastructure.Messaging.IntegrationEvents
{
    public class UserProfileUpdatedIntegrationEvent
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OccurredOn { get; set; }

        public UserProfileUpdatedIntegrationEvent(Guid profileId, Guid userId)
        {
            ProfileId = profileId;
            UserId = userId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
