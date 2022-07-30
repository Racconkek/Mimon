using Mimon.Api.Dto;
using Mimon.Api.Dto.Photos;

namespace Mimon.BusinessLogic.Repositories.Photos;

public interface IPhotosRepository
{
    Task<Photo> Read(Guid id);
    Task<Guid> Create(Photo photo);
    Task<Photo[]> ReadAllFromUser(Guid userId, int skip = 0, int take = 20);
    Task<Photo[]> ReadAllFromUsers(Guid[] userIds, int skip = 0, int take = 20);
}