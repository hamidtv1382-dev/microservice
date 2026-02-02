using Media_Service.src._01_Domain.Services.Interfaces;

namespace Media_Service.src._03._Infrastructure.Storage
{
    public class CloudFileStorageService : IFileStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _serviceBaseUrl;
        private readonly ILoggingService _logger;

        public CloudFileStorageService(HttpClient httpClient, IConfiguration configuration, ILoggingService logger)
        {
            _httpClient = httpClient;
            _serviceBaseUrl = configuration["Storage:CloudServiceUrl"] ?? "http://localhost:5001/api/files";
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(string relativePath, byte[] fileData)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(fileData), "file", Path.GetFileName(relativePath));
            content.Add(new StringContent(relativePath), "path");

            var response = await _httpClient.PostAsync($"{_serviceBaseUrl}/upload", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"File uploaded to cloud: {result}");
                return result; // Returns the absolute URL
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Cloud upload failed: {errorMessage}");
            throw new Exception("Failed to upload file to cloud storage.");
        }

        public Task<string> GetAbsoluteUrlAsync(string relativePath)
        {
            // Assuming cloud storage generates a permanent URL
            var absoluteUrl = $"{_serviceBaseUrl}/{relativePath}";
            return Task.FromResult(absoluteUrl);
        }

        public async Task<bool> DeleteFileAsync(string relativePath)
        {
            var response = await _httpClient.DeleteAsync($"{_serviceBaseUrl}/delete?path={relativePath}");
            _logger.LogInformation($"Cloud delete request status: {response.StatusCode}");
            return await Task.FromResult(response.IsSuccessStatusCode);
        }
    }
}
