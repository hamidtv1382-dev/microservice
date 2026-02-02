using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Review_Rating_Service.src._01_Domain.Core.Interfaces.Repositories;
using Review_Rating_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Review_Rating_Service.src._01_Domain.Services.Implementations;
using Review_Rating_Service.src._01_Domain.Services.Interfaces;
using Review_Rating_Service.src._02_Application.Interfaces;
using Review_Rating_Service.src._02_Application.Services.Implementations;
using Review_Rating_Service.src._02_Application.Services.Interfaces;
using Review_Rating_Service.src._03_Infrastructure.Data;
using Review_Rating_Service.src._03_Infrastructure.Repositories;
using Review_Rating_Service.src._03_Infrastructure.Services.External;
using System.Security.Claims; // اضافه شده برای ClaimTypes
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Review_Rating_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReviewServices(this IServiceCollection services, string connectionString)
        {
            // ==========================================
            // 1. Database & Identity Configuration
            // ==========================================

            // تنظیمات دیتابیس
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // تنظیمات ASP.NET Core Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // ==========================================
            // 2. Authentication (مشابه Catalog Service)
            // ==========================================

            // فراخوانی متد تنظیم احراز هویت
            services.AddAppAuthentication(services.BuildServiceProvider().GetRequiredService<IConfiguration>());

            // ==========================================
            // 3. Repositories, Services & UoW
            // ==========================================

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();

            services.AddScoped<IReviewApplicationService, ReviewApplicationService>();
            services.AddScoped<IRatingApplicationService, RatingApplicationService>();

            services.AddScoped<IReviewDomainService, ReviewDomainService>();
            services.AddScoped<IRatingDomainService, RatingDomainService>();

            services.AddHttpClient<IExternalNotificationService, NotificationClient>();

            services.AddAutoMapper(config =>
            {
                config.AddMaps(System.Reflection.Assembly.GetExecutingAssembly());
            });

            return services;
        }

        // ==========================================
        // متد جداگانه برای تنظیم احراز هویت (مانند Catalog Service)
        // ==========================================
        public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),

                    // *** تنظیم ClaimTypes برای یکسان بودن با مایکروسافت ***
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name
                };
            });

            return services;
        }
    }
}