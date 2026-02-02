using Microsoft.EntityFrameworkCore;
using Wallet_Service.src._01_Domain.Core.Interfaces.Repositories;
using Wallet_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Wallet_Service.src._01_Domain.Services.Implementations;
using Wallet_Service.src._01_Domain.Services.Interfaces;
using Wallet_Service.src._02_Application.Interfaces;
using Wallet_Service.src._02_Application.Mappings;
using Wallet_Service.src._02_Application.Services.Implementations;
using Wallet_Service.src._02_Application.Services.Interfaces;
using Wallet_Service.src._03_Infrastructure.Data;
using Wallet_Service.src._03_Infrastructure.Repositories;
using Wallet_Service.src._03_Infrastructure.Services.Caching;
using Wallet_Service.src._03_Infrastructure.Services.External;

namespace Wallet_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWalletServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Explicitly register Repositories
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();

            // Mapping Profile
            services.AddScoped<WalletMappingProfile>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application Services
            services.AddScoped<IWalletApplicationService, WalletApplicationService>();
            services.AddScoped<IWalletTransactionApplicationService, WalletTransactionApplicationService>();

            // Domain Services
            services.AddScoped<IWalletDomainService, WalletDomainService>();
            services.AddScoped<ITransactionDomainService, TransactionDomainService>();

            // Infrastructure Services
            services.AddScoped<IExternalPaymentGateway, PaymentGatewayClient>();
            services.AddSingleton<ICacheService, RedisCacheService>();

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
