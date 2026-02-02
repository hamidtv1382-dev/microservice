namespace User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile
{
    public class UserPreference
    {
        public string LanguageCode { get; private set; } // en, fa, etc.
        public string CurrencyCode { get; private set; } // USD, IRR, etc.
        public bool NotificationsEnabled { get; private set; }
        public string Theme { get; private set; } // Light, Dark

        public UserPreference(string languageCode, string currencyCode, bool notificationsEnabled = true, string theme = "Light")
        {
            LanguageCode = languageCode ?? "en";
            CurrencyCode = currencyCode ?? "USD";
            NotificationsEnabled = notificationsEnabled;
            Theme = theme;
        }

        public void UpdateLanguage(string languageCode)
        {
            LanguageCode = languageCode;
        }

        public void UpdateCurrency(string currencyCode)
        {
            CurrencyCode = currencyCode;
        }

        public void ToggleNotifications(bool enabled)
        {
            NotificationsEnabled = enabled;
        }

        public void UpdateTheme(string theme)
        {
            Theme = theme;
        }
    }
}
