using Media_Service.src._01_Domain.Services.Interfaces;

namespace Media_Service.src._03._Infrastructure.Storage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _baseDirectory;
        private readonly ILoggingService _logger;

        public LocalFileStorageService(IConfiguration configuration, ILoggingService logger)
        {
            _baseDirectory = configuration["Storage:LocalPath"] ?? "AppData/Media";
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(string relativePath, byte[] fileData)
        {
            var fullPath = Path.Combine(_baseDirectory, relativePath);
            var directory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllBytesAsync(fullPath, fileData);
            _logger.LogInformation($"File saved to: {fullPath}");

            return fullPath;
        }

        public Task<string> GetAbsoluteUrlAsync(string relativePath)
        {
            var fullPath = Path.Combine(_baseDirectory, relativePath);
            return Task.FromResult(fullPath);
        }

        public async Task<bool> DeleteFileAsync(string relativePath)
        {
            var fullPath = Path.Combine(_baseDirectory, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation($"File deleted: {fullPath}");
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
