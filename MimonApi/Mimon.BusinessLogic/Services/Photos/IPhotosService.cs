using Mimon.Api.Dto;
using Mimon.Api.Dto.Photos;

namespace Mimon.BusinessLogic.Services.Photos;

public interface IPhotosService
{
    Task<Photo> GetPhotoData(Guid photoId);
    Task<byte[]> GetPhotoBytes(Guid photoId);
    Task<Guid> CreatePhoto(Photo photo);
    Task CreatePhotoFile(Guid photoId, byte[] bytes);
    Task<Photo[]> GetFeedForUser(Guid userId, int skip = 0, int take = 20);
    Task<Reaction[]> GetPhotoReactions(Guid photoId);
    Task CreateReaction(Reaction reaction);
    Task<bool> DeleteReaction(Reaction reaction);
}