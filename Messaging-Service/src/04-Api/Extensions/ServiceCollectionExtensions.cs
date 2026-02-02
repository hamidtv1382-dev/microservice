using Messaging_Service.src._01_Domain.Core.Interfaces.Repositories;
using Messaging_Service.src._01_Domain.Core.Interfaces.Services;
using Messaging_Service.src._02_Application.Services.Implementations;
using Messaging_Service.src._02_Application.Services.Interfaces;
using Messaging_Service.src._03_Infrastructure.Data;
using Messaging_Service.src._03_Infrastructure.Repositories;
using Messaging_Service.src._03_Infrastructure.Services.Moderation;
using Messaging_Service.src._03_Infrastructure.Services.NotificationGateway;
using Microsoft.EntityFrameworkCore;

namespace Messaging_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MessagingDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IModerationService, ContentModerationService>();

            // Register Notification Gateway implementations
            services.AddScoped<INotificationService>(provider =>
            {
                // In a real scenario, use strategy pattern or factory to pick the correct service
                // Here we return a composite or a specific one. 
                // For this exercise, let's return the Push client as the default implementation 
                // or a composite that handles all.
                return new PushNotificationClient();
            });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageApplicationService, MessageApplicationService>();
            services.AddScoped<IChatApplicationService, ChatApplicationService>();
            services.AddScoped<INotificationApplicationService, NotificationApplicationService>();

            return services;
        }
    }
}
