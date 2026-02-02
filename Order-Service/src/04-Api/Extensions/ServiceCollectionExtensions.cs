using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Order_Service.src._01_Domain.Services.Implementations;
using Order_Service.src._01_Domain.Services.Interfaces;
using Order_Service.src._02_Application.Interfaces;
using Order_Service.src._02_Application.Services.Implementations;
using Order_Service.src._02_Application.Services.Interfaces;
using Order_Service.src._03_Infrastructure.Caching;
using Order_Service.src._03_Infrastructure.Data;
using Order_Service.src._03_Infrastructure.Repositories;
using Order_Service.src._03_Infrastructure.Services.External;
using Order_Service.src._04_Api.Security;
using StackExchange.Redis;
using System.Security.Claims;

namespace Order_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrderApplicationService, OrderApplicationService>();
            services.AddScoped<IBasketApplicationService, BasketApplicationService>();
            services.AddScoped<IPaymentApplicationService, PaymentApplicationService>();
            services.AddScoped<IRefundApplicationService, RefundApplicationService>();
            services.AddScoped<IAdminDiscountApplicationService, AdminDiscountApplicationService>();

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderDomainService, OrderDomainService>();
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddScoped<IJwtTokenValidationService, JwtTokenValidationService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IRefundRepository, RefundRepository>();
            services.AddScoped<IAdminDiscountRepository, AdminDiscountRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // External HTTP Clients with BaseAddress
            services.AddHttpClient<CatalogServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:CatalogService"]);
            });

            services.AddHttpClient<PaymentServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:PaymentService"]);
            });

            services.AddHttpClient<SellerFinanceServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:SellerFinanceService"]);
            });

            // Payment Gateway Implementation
            services.AddScoped<IPaymentGateway, PaymentGatewayService>();

            // Redis ConnectionMultiplexer
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConfig = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(redisConfig);
            });

            // Redis Cache Service
            services.AddSingleton<ICacheService, RedisCacheService>();

            return services;
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
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

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = "user_id"
                    };

                    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                    {
                        OnTokenValidated = ctx =>
                        {
                            Console.WriteLine("Token validated. Claims:");
                            foreach (var c in ctx.Principal.Claims)
                                Console.WriteLine($"{c.Type} = {c.Value}");
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(options =>
            {
                AuthorizationPolicies.Configure(options);
            });

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

    }
}
