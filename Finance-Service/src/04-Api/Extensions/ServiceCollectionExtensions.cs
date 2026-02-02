using Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Finance_Service.src._01_Domain.Services.Implementations;
using Finance_Service.src._01_Domain.Services.Interfaces;
using Finance_Service.src._02_Application.Interfaces;
using Finance_Service.src._02_Application.Mappings;
using Finance_Service.src._02_Application.Services.Implementations;
using Finance_Service.src._02_Application.Services.Interfaces;
using Finance_Service.src._03_Infrastructure.Caching;
using Finance_Service.src._03_Infrastructure.Data;
using Finance_Service.src._03_Infrastructure.Services.External;
using Microsoft.EntityFrameworkCore;

namespace Finance_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFinanceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Mapping Profile
            services.AddScoped<FinanceMappingProfile>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application Services
            services.AddScoped<IFeeApplicationService, FeeApplicationService>();
            services.AddScoped<ICommissionApplicationService, CommissionApplicationService>();
            services.AddScoped<ISettlementApplicationService, SettlementApplicationService>();
            services.AddScoped<IFinanceApplicationService, FinanceApplicationService>();

            // Domain Services
            services.AddScoped<IFinanceDomainService, FinanceDomainService>();

            // Infrastructure Services
            services.AddScoped<IExternalPaymentService, PaymentGatewayClient>();
            services.AddScoped<IOrderServiceClient, OrderServiceClient>();
            services.AddSingleton<ICacheService, RedisCacheService>();

            // Register HttpClients with Base Addresses
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