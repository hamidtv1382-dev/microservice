using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Messaging_Service.src._01_Domain.Core.Interfaces.Services;

namespace Messaging_Service.src._03_Infrastructure.Services.NotificationGateway
{
    public class PushNotificationClient : INotificationService
    {
        public PushNotificationClient()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var credential = GoogleCredential.FromFile("firebase-key.json"); // مسیر فایل کلید شما
                FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }
        }

        public async Task SendPushAsync(Guid userId, string title, string message)
        {
            string userDeviceToken = await GetUserDeviceTokenAsync(userId);

            if (string.IsNullOrEmpty(userDeviceToken))
            {
                return;
            }

            var messagePayload = new Message()
            {
                Token = userDeviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = message
                },
                Data = new Dictionary<string, string>()
                {
                    ["UserId"] = userId.ToString(),
                    ["Type"] = "ChatMessage"
                },
                Android = new AndroidConfig()
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification()
                    {
                        ChannelId = "high_importance_channel", 
                        Sound = "default"
                    }
                },
                Apns = new ApnsConfig()
                {
                    Aps = new Aps()
                    {
                        Sound = "default",
                        Badge = 1
                    }
                }
            };

            try
            {
                var messaging = FirebaseMessaging.DefaultInstance;
                string response = await messaging.SendAsync(messagePayload);

                Console.WriteLine($"[Push Gateway] Successfully sent message: {response}");
            }
            catch (FirebaseMessagingException ex)
            {
                Console.WriteLine($"[Push Gateway] Error sending message: {ex.Message}");
            }
        }

        private async Task<string> GetUserDeviceTokenAsync(Guid userId)
        {
            return await Task.FromResult("dummy_device_token_placeholder");
        }

        public Task SendSmsAsync(string phoneNumber, string message)
        {
            throw new NotSupportedException("PushNotificationClient does not support SMS.");
        }

        public Task SendEmailAsync(string email, string subject, string body)
        {
            throw new NotSupportedException("PushNotificationClient does not support Email.");
        }
    }
}
