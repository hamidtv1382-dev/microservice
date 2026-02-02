namespace User_Profile_Service.src._01_Domain.Core.Events
{
    public class UserProfileUpdatedEvent
    {
        public Guid ProfileId { get; set; }
        public DateTime OccurredOn { get; set; }

        public UserProfileUpdatedEvent(Guid profileId)
        {
            ProfileId = profileId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
