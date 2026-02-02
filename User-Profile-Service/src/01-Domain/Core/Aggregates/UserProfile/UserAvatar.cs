namespace User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile
{
    public class UserAvatar
    {
        public string Url { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }

        public UserAvatar(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Avatar URL cannot be empty.", nameof(url));

            Url = url;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void UpdateUrl(string newUrl)
        {
            if (string.IsNullOrWhiteSpace(newUrl))
                throw new ArgumentException("Avatar URL cannot be empty.", nameof(newUrl));

            Url = newUrl;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
