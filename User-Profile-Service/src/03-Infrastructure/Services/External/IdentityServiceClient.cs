using User_Profile_Service.src._02_Application.Interfaces;

namespace User_Profile_Service.src._03_Infrastructure.Services.External
{
    public class IdentityServiceClient : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidateUserExistsAsync(Guid userId)
        {
            // Simulate call to Identity Service
            var response = await _httpClient.GetAsync($"/api/identity/users/{userId}/exists");
            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetUserEmailAsync(Guid userId)
        {
            // Simulate call to Identity Service
            var response = await _httpClient.GetAsync($"/api/identity/users/{userId}/email");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
    }
}
