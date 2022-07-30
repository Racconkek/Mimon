using Microsoft.EntityFrameworkCore;
using Mimon.Api.Dto.Photos;
using Mimon.BusinessLogic.Repositories.Database;

namespace Mimon.BusinessLogic.Repositories.Photos;

public class PhotosRepository : IPhotosRepository
{
    public PhotosRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }
    
    public async Task<Photo> Read(Guid id)
    {
        var result =  await databaseContext.PhotosStorage.FirstAsync(x => x.Id == id);

        return ToModel(result);
    }

    public async Task<Guid> Create(Photo photo)
    {
        await databaseContext.PhotosStorage.AddAsync(ToStorageElement(photo));
        await databaseContext.SaveChangesAsync();

        return photo.Id!.Value;
    }

    public async Task<Photo[]> ReadAllFromUser(Guid userId, int skip = 0, int take = 20)
    {
        var results = await databaseContext.PhotosStorage
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreationDateTime)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();

        return results.Select(ToModel).ToArray();
    }

    public async Task<Photo[]> ReadAllFromUsers(Guid[] userIds, int skip = 0, int take = 20)
    {
        var results = await databaseContext.PhotosStorage
            .Where(x => userIds.Contains(x.UserId))
            .OrderByDescending(x => x.CreationDateTime)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();

        return results.Select(ToModel).ToArray();
    }

    private static Photo ToModel(PhotoStorageElement storageElement)
    {
        return new Photo
        {
            Id = storageElement.Id,
            UserId = storageElement.UserId,
            CreationDateTime = storageElement.CreationDateTime
        };
    }

    private static PhotoStorageElement ToStorageElement(Photo photo)
    {
        return new PhotoStorageElement
        {
            Id = photo.Id ?? Guid.NewGuid(),
            UserId = photo.UserId,
            CreationDateTime = photo.CreationDateTime ?? DateTime.Now
        };
    }
    
    private readonly DatabaseContext databaseContext;
}