namespace User_Profile_Service.src._01_Domain.Core.Events
{
    public class UserProfileCreatedEvent
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OccurredOn { get; set; }

        public UserProfileCreatedEvent(Guid profileId, Guid userId)
        {
            ProfileId = profileId;
            UserId = userId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
