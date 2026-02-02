using Microsoft.EntityFrameworkCore;
using Notification_Services.src._01_Domain.Core.Interfaces.Repositories;
using Notification_Services.src._01_Domain.Core.Interfaces.UnitOfWork;
using Notification_Services.src._01_Domain.Services.Implementations;
using Notification_Services.src._01_Domain.Services.Interfaces;
using Notification_Services.src._02_Application.Interfaces;
using Notification_Services.src._02_Application.Services.Implementations;
using Notification_Services.src._02_Application.Services.Interfaces;
using Notification_Services.src._03_Infrastructure.Data;
using Notification_Services.src._03_Infrastructure.Repositories;
using Notification_Services.src._03_Infrastructure.Services.External;
using static System.Net.Mime.MediaTypeNames;

namespace Notification_Services.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services, string connectionString)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Repositories & Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();

            // Application Services
            services.AddScoped<INotificationApplicationService, NotificationApplicationService>();
            services.AddScoped<ITemplateApplicationService, TemplateApplicationService>();

            // Domain Services
            services.AddScoped<INotificationDomainService, NotificationDomainService>();
            services.AddScoped<ITemplateDomainService, TemplateDomainService>();

            // External Services (Inject specific clients for each type)
            services.AddHttpClient<IExternalMessagingGateway, EmailClient>();
            // services.AddHttpClient<Application.Interfaces.IExternalMessagingGateway, SmsClient>();
            // services.AddHttpClient<Application.Interfaces.IExternalMessagingGateway, PushClient>();

            // AutoMapper
            services.AddAutoMapper(config =>
            {
                config.AddMaps(System.Reflection.Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
