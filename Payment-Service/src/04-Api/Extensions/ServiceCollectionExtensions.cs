using Microsoft.EntityFrameworkCore;
using Payment_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Payment_Service.src._01_Domain.Services.Implementations;
using Payment_Service.src._01_Domain.Services.Interfaces;
using Payment_Service.src._02_Application.Interfaces;
using Payment_Service.src._02_Application.Mappings;
using Payment_Service.src._02_Application.Services.Implementations;
using Payment_Service.src._02_Application.Services.Interfaces;
using Payment_Service.src._03_Infrastructure.Caching;
using Payment_Service.src._03_Infrastructure.Data;
using Payment_Service.src._03_Infrastructure.Services.External;

namespace Payment_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database - Configure DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // HttpContextAccessor (برای خواندن توکن در OrderServiceClient)
            services.AddHttpContextAccessor();

            // Mapping Profile
            services.AddScoped<PaymentMappingProfile>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application Services
            services.AddScoped<IPaymentApplicationService, PaymentApplicationService>();
            services.AddScoped<IRefundApplicationService, RefundApplicationService>();

            // Domain Services
            services.AddScoped<IPaymentDomainService, PaymentDomainService>();
            services.AddScoped<IRefundDomainService, RefundDomainService>();

            // Infrastructure Services (External Clients)
            services.AddScoped<IPaymentGateway, PaymentGatewayClient>();
            services.AddScoped<IWalletServiceClient, WalletServiceClient>();
            services.AddScoped<IOrderServiceClient, OrderServiceClient>();
            services.AddSingleton<ICacheService, RedisCacheService>();

            // Register HttpClients with Base Addresses
            services.AddHttpClient<WalletServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:WalletService"] ?? "http://localhost:5004/api");
            });

            services.AddHttpClient<OrderServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:OrderService"] ?? "http://localhost:5288/api");
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}