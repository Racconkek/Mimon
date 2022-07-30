using Mimon.Api.Dto.Photos;
using Mimon.BusinessLogic.Repositories.Photos;
using Mimon.BusinessLogic.Repositories.Reactions;
using Mimon.BusinessLogic.Services.Users;

namespace Mimon.BusinessLogic.Services.Photos;

public class PhotosService : IPhotosService
{
    public PhotosService(
        IUsersService usersService,
        IPhotosRepository photosRepository,
        IPhotoDataRepository photoDataRepository,
        IReactionsRepository reactionsRepository
    )
    {
        this.usersService = usersService;
        this.photosRepository = photosRepository;
        this.photoDataRepository = photoDataRepository;
        this.reactionsRepository = reactionsRepository;
    }

    public async Task<Photo> GetPhotoData(Guid photoId)
    {
        return await photosRepository.Read(photoId);
    }

    public Task<byte[]> GetPhotoBytes(Guid photoId)
    {
        return photoDataRepository.Read(photoId);
    }

    public async Task<Guid> CreatePhoto(Photo photo)
    {
        return await photosRepository.Create(photo);
    }

    public async Task CreatePhotoFile(Guid photoId, byte[] bytes)
    {
        await photoDataRepository.Write(photoId, bytes);
    }

    public async Task<Photo[]> GetFeedForUser(Guid userId, int skip = 0, int take = 20)
    {
        var userFriends = await usersService.FindUserFriends(userId, true);
        return await photosRepository.ReadAllFromUsers(userFriends.Select(x => x.Friend.Id!.Value).ToArray(), skip, take);
    }

    public async Task<Reaction[]> GetPhotoReactions(Guid photoId)
    {
        return await reactionsRepository.ReadAll(photoId);
    }

    public async Task CreateReaction(Reaction reaction)
    {
        await reactionsRepository.Create(reaction);
    }

    public async Task<bool> DeleteReaction(Reaction reaction)
    {
        return await reactionsRepository.TryDelete(reaction);
    }
    
    private readonly IUsersService usersService;
    private readonly IPhotosRepository photosRepository;
    private readonly IPhotoDataRepository photoDataRepository;
    private readonly IReactionsRepository reactionsRepository;
}