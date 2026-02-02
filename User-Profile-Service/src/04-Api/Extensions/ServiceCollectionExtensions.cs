using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using User_Profile_Service.src._01_Domain.Core.Interfaces.Repositories;
using User_Profile_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using User_Profile_Service.src._02_Application.Interfaces;
using User_Profile_Service.src._02_Application.Services.Implementations;
using User_Profile_Service.src._02_Application.Services.Interfaces;
using User_Profile_Service.src._03_Infrastructure.Data;
using User_Profile_Service.src._03_Infrastructure.Repositories;
using User_Profile_Service.src._03_Infrastructure.Services.External;
using static System.Net.Mime.MediaTypeNames;

namespace User_Profile_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserProfileServices(this IServiceCollection services, string connectionString)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Repositories & Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            // Application Services
            services.AddScoped<IUserProfileApplicationService, UserProfileApplicationService>();
            services.AddScoped<IUserAddressApplicationService, UserAddressApplicationService>();
            services.AddScoped<IUserPreferenceApplicationService, UserPreferenceApplicationService>();

            // External Services
            services.AddHttpClient<IIdentityService, IdentityServiceClient>();
            services.AddHttpClient<IMediaService, MediaServiceClient>();

            // AutoMapper (اصلاح شده برای سازگاری با نسخه‌های مختلف)
            // این خط به صورت خودکار تمامی Profile ها را در اسمبلی جاری پیدا می‌کند
            services.AddAutoMapper(config =>
            {
                config.AddMaps(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
