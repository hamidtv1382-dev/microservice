namespace User_Profile_Service.src._02_Application.DTOs.Responses
{
    public class UserPreferenceResponseDto
    {
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public bool NotificationsEnabled { get; set; }
        public string Theme { get; set; }
    }
}
