using Media_Service.src._01_Domain.Core.Interfaces.Repositories;
using Media_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Media_Service.src._01_Domain.Services.Implementations;
using Media_Service.src._01_Domain.Services.Interfaces;
using Media_Service.src._02_Application.Interfaces;
using Media_Service.src._02_Application.Services.Implementations;
using Media_Service.src._02_Application.Services.Interfaces;
using Media_Service.src._03._Infrastructure.Caching;
using Media_Service.src._03._Infrastructure.Data;
using Media_Service.src._03._Infrastructure.Repositories;
using Media_Service.src._03._Infrastructure.Services.External;
using Media_Service.src._03._Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace Media_Service.src._04_Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediaServices(this IServiceCollection services, string connectionString)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Repositories & Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
            services.AddScoped<IMediaFolderRepository, MediaFolderRepository>();

            // Domain Services
            services.AddScoped<IMediaDomainService, MediaDomainService>();
            services.AddScoped<IMediaClassificationService, MediaClassificationService>();

            // Application Services
            services.AddScoped<IMediaUploadApplicationService, MediaUploadApplicationService>();
            services.AddScoped<IMediaFolderApplicationService, MediaFolderApplicationService>();
            services.AddScoped<IMediaQueryApplicationService, MediaQueryApplicationService>();

            // External Catalog Services
            services.AddHttpClient<IBrandCatalogService, BrandCatalogClient>();
            services.AddHttpClient<ICategoryCatalogService, CategoryCatalogClient>();
            services.AddHttpClient<IProductCatalogService, ProductCatalogClient>();

            // Storage Services
            // You can choose between LocalFileStorageService and CloudFileStorageService here
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IStoragePathResolver, StoragePathResolver>();

            // Caching
            services.AddSingleton<ICacheService, RedisCacheService>();

            // Internal Services
            services.AddScoped<ILoggingService, LoggingService>();

            // AutoMapper
            services.AddAutoMapper(config =>
            {
                config.AddMaps(System.Reflection.Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
