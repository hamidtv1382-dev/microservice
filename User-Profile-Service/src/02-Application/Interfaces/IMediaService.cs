namespace User_Profile_Service.src._02_Application.Interfaces
{
    public interface IMediaService
    {
        Task<string> SaveAvatarAsync(Guid userId, byte[] fileData, string extension);
        Task<bool> DeleteMediaAsync(string mediaUrl);
    }
}
