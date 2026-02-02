using Microsoft.EntityFrameworkCore;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Seller_Finance_Service.src._01_Domain.Services.Implementations;
using Seller_Finance_Service.src._01_Domain.Services.Interfaces;
using Seller_Finance_Service.src._02_Application.Interfaces;
using Seller_Finance_Service.src._02_Application.Mappings;
using Seller_Finance_Service.src._02_Application.Services.Implementations;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;
using Seller_Finance_Service.src._03_Infrastructure.Data;
using Seller_Finance_Service.src._03_Infrastructure.Repositories;
using Seller_Finance_Service.src._03_Infrastructure.Services.Caching;
using Seller_Finance_Service.src._03_Infrastructure.Services.External;

namespace Seller_Finance_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSellerFinanceServices(this IServiceCollection services, string connectionString)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // --- Mapping Profile ---
            // Register the Mapping Profile so it can be injected (Manual Mapping Logic)
            services.AddScoped<SellerFinanceMappingProfile>();
            // ------------------------

            // Domain & Infrastructure Services
            services.AddScoped<ISellerFinanceDomainService, SellerFinanceDomainService>();
            services.AddScoped<IPayoutPolicyService, PayoutPolicyService>();
            services.AddScoped<ILoggingService, LoggingService>();

            // Application Services
            // Existing Services
            services.AddScoped<ISellerBalanceApplicationService, SellerBalanceApplicationService>();
            services.AddScoped<ISellerPayoutApplicationService, SellerPayoutApplicationService>();
            services.AddScoped<ISellerTransactionApplicationService, SellerTransactionApplicationService>();

            // --- NEW SERVICE ADDED ---
            services.AddScoped<ISellerAccountApplicationService, SellerAccountApplicationService>();
            // --------------------------

            // Repositories & Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISellerAccountRepository, SellerAccountRepository>();
            services.AddScoped<ISellerTransactionRepository, SellerTransactionRepository>();
            services.AddScoped<ISellerPayoutRepository, SellerPayoutRepository>();
            services.AddScoped<ISellerHoldRepository, SellerHoldRepository>();

            // External Services (HTTP Clients)
            services.AddHttpClient<IFinanceService, FinanceServiceClient>();
            services.AddHttpClient<INotificationService, NotificationServiceClient>();
            services.AddHttpClient<IWalletService, WalletServiceClient>();

            // Caching
            services.AddSingleton<ICacheService, RedisCacheService>();

            return services;
        }
    }
}