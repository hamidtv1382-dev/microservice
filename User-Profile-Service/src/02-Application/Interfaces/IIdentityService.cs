namespace User_Profile_Service.src._02_Application.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> ValidateUserExistsAsync(Guid userId);
        Task<string> GetUserEmailAsync(Guid userId);
    }
}
