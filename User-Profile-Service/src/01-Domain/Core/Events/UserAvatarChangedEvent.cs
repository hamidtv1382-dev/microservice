namespace User_Profile_Service.src._01_Domain.Core.Events
{
    public class UserAvatarChangedEvent
    {
        public Guid ProfileId { get; set; }
        public string NewAvatarUrl { get; set; }
        public DateTime OccurredOn { get; set; }

        public UserAvatarChangedEvent(Guid profileId, string newAvatarUrl)
        {
            ProfileId = profileId;
            NewAvatarUrl = newAvatarUrl;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
