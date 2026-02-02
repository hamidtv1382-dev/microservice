using Review_Rating_Service.src._02_Application.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Review_Rating_Service.src._03_Infrastructure.Services.External
{
    public class NotificationClient : IExternalNotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            // تنظیم BaseAddress برای تماس‌های REST
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://localhost:7231");
            }
        }

        // ---------------------------------------------------------------------
        // DTOهای داخلی برای تطابق دقیق با قرارداد سرویس نوتیفیکیشن
        // ---------------------------------------------------------------------
        internal class CreateNotificationRequestDto
        {
            [Required]
            public int Type { get; set; } // 1: Email, 2: SMS, 3: Push

            [Required]
            [MaxLength(200)]
            public string Title { get; set; }

            [Required]
            [MaxLength(5000)]
            public string Message { get; set; }

            [Required]
            public List<RecipientDto> Recipients { get; set; } = new List<RecipientDto>();

            public List<string> AttachmentUrls { get; set; } = new List<string>();

            public DateTime? ScheduledAt { get; set; }
        }

        internal class RecipientDto
        {
            [Required]
            public int Type { get; set; } // 1: User, 2: Seller, 3: Admin, 4: System

            [Required]
            public string Contact { get; set; }
        }

        // ---------------------------------------------------------------------
        // پیاده‌سازی متدهای بیزنسی
        // ---------------------------------------------------------------------

        public async Task NotifySellerOnReviewCreatedAsync(int productId, string reviewText)
        {
            try
            {
                // 1. ساخت پیام بر اساس بیزنس ریویو
                var notificationPayload = new CreateNotificationRequestDto
                {
                    // نوع ارسال: ایمیل (استاندارد برای اطلاع‌رسانی فروشگاه)
                    Type = 1,

                    // عنوان پیام
                    Title = $"New Review Received - Product #{productId}",

                    // محتوای پیام (ترکیب متن ریویو)
                    Message = $"Hello Seller,\nA new review has been submitted for your product (ID: {productId}).\n\nReview Text: \"{reviewText}\"",

                    // لیست گیرندگان (در محیط واقعی، اینجا باید ایمیل فروشنده از دیتابیس خوانده شود)
                    // فعلاً از یک Placeholder معتبر استفاده می‌کنیم
                    Recipients = new List<RecipientDto>
                    {
                        new RecipientDto
                        { 
                            // نوع گیرنده: فروشنده
                            Type = 2, 
                            // ایمیل فروشنده (فرض: باید توسط سرویس Product پر شود)
                            Contact = $"seller-product-{productId}@shop.example.com"
                        }
                    },

                    // لیست پیوست‌ها (اختیاری)
                    AttachmentUrls = new List<string>
                    {
                        // مثلاً لینک به عکس محصول یا ریویو
                        // "https://example.com/reviews/..." 
                    },

                    // زمان‌بندی: ارسال فوری (Null)
                    ScheduledAt = null
                };

                // 2. ارسال درخواست به سرویس نوتیفیکیشن
                var response = await _httpClient.PostAsJsonAsync("/api/notifications", notificationPayload);

                // 3. مدیریت وضعیت پاسخ
                if (!response.IsSuccessStatusCode)
                {
                    // خواندن محتوای خطا برای لاگینگ بهتر
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[NotificationClient Error] Failed to notify seller. Status: {response.StatusCode}. Details: {errorDetails}");
                }
                else
                {
                    Console.WriteLine($"[NotificationClient] Successfully notified seller for Product {productId}.");
                }
            }
            catch (Exception ex)
            {
                // جلوگیری از کرش کردن سرویس اصلی در صورت بروز مشکل در شبکه یا سرویس نوتیفیکیشن
                Console.WriteLine($"[NotificationClient Exception] {ex.Message}");
            }
        }

        public async Task NotifyAdminOnPendingReviewAsync(Guid reviewId)
        {
            try
            {
                // ساخت پیلود برای ادمین
                var notificationPayload = new CreateNotificationRequestDto
                {
                    Type = 1, // Email
                    Title = "New Pending Review Requires Approval",
                    Message = $"A new review (ID: {reviewId}) is pending and requires admin approval.",

                    // گیرنده: ادمین سیستم
                    Recipients = new List<RecipientDto>
                    {
                        new RecipientDto
                        {
                            Type = 3, // Admin
                            Contact = "admin@system.example.com"
                        }
                    },

                    AttachmentUrls = null,
                    ScheduledAt = null
                };

                var response = await _httpClient.PostAsJsonAsync("/api/notifications", notificationPayload);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[NotificationClient Error] Failed to notify admin. Status: {response.StatusCode}. Details: {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[NotificationClient Exception] {ex.Message}");
            }
        }
    }
}