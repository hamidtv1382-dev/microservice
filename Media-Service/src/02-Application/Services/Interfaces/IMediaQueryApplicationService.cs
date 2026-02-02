using Media_Service.src._02_Application.DTOs.Requests;
using Media_Service.src._02_Application.DTOs.Responses;

namespace Media_Service.src._02_Application.Services.Interfaces
{
    public interface IMediaQueryApplicationService
    {
        Task<MediaPathResponseDto> ResolvePathAsync(ResolveMediaPathRequestDto request);
        Task<MediaUploadResponseDto> GetMediaAsync(Guid fileId);
    }
}
