using User_Profile_Service.src._02_Application.Interfaces;

namespace User_Profile_Service.src._03_Infrastructure.Services.External
{
    public class MediaServiceClient : IMediaService
    {
        private readonly HttpClient _httpClient;

        public MediaServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SaveAvatarAsync(Guid userId, byte[] fileData, string extension)
        {
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(fileData), "file", $"avatar_{userId}{extension}");

            var response = await _httpClient.PostAsync("/api/media/upload", content);
            if (response.IsSuccessStatusCode)
            {
                var url = await response.Content.ReadAsStringAsync();
                return url;
            }
            throw new Exception("Failed to upload media to Media Service.");
        }

        public async Task<bool> DeleteMediaAsync(string mediaUrl)
        {
            var response = await _httpClient.DeleteAsync($"/api/media?url={mediaUrl}");
            return response.IsSuccessStatusCode;
        }
    }
}
