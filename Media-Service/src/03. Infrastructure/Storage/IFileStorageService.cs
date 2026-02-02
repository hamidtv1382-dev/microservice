namespace Media_Service.src._03._Infrastructure.Storage
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(string relativePath, byte[] fileData);
        Task<string> GetAbsoluteUrlAsync(string relativePath);
        Task<bool> DeleteFileAsync(string relativePath);
    }
}
