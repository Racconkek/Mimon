using Mimon.Api.Dto;
using Mimon.Api.Dto.Photos;

namespace Mimon.BusinessLogic.Repositories.Photos;

public class FakePhotosRepository : IPhotosRepository
{
    public FakePhotosRepository()
    {
        storage = new Dictionary<Guid, Photo>();
    }
    
    public Task<Photo> Read(Guid id)
    {
        return Task.FromResult(storage[id]);
    }

    public Task<Guid> Create(Photo photo)
    {
        photo.Id ??= Guid.NewGuid();
        storage[photo.Id.Value] = photo;

        return Task.FromResult(photo.Id.Value);
    }

    public Task<Photo[]> ReadAllFromUser(Guid userId, int skip = 0, int take = 20)
    {
        var results = storage.Values
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreationDateTime)
            .Skip(skip)
            .Take(take)
            .ToArray();

        return Task.FromResult(results);
    }

    public Task<Photo[]> ReadAllFromUsers(Guid[] userIds, int skip = 0, int take = 20)
    {
        var results = storage.Values
            .Where(x => userIds.Contains(x.UserId))
            .OrderByDescending(x => x.CreationDateTime)
            .Skip(skip)
            .Take(take)
            .ToArray();

        return Task.FromResult(results);
    }

    private readonly Dictionary<Guid, Photo> storage;
}