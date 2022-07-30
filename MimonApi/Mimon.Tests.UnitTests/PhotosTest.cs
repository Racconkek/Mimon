using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mimon.Api.Dto.Photos;
using Mimon.Api.Dto.Users;
using Mimon.BusinessLogic.Repositories.Photos;
using Mimon.BusinessLogic.Repositories.Reactions;
using Mimon.BusinessLogic.Repositories.Users;
using Mimon.BusinessLogic.Repositories.UsersRelations;
using Mimon.BusinessLogic.Services.Photos;
using Mimon.BusinessLogic.Services.Users;
using Mimon.Tests.Common.Extensions;
using Mimon.Tests.Common.Generators;
using NUnit.Framework;

namespace Mimon.Tests.UnitTests;

public class PhotosTest
{
    [SetUp]
    public void Setup()
    {
        usersService = new UsersService(new FakeUsersRepository(), new FakeRelationsRepository());
        photosRepository = new FakePhotosRepository();
        photosService = new PhotosService(usersService, photosRepository, new PhotoDataRepository(true), new FakeReactionsRepository());
        user = UserGenerator.Generate();
        usersService.CreateOrUpdate(user).GetAwaiter().GetResult();
    }

    [Test]
    public async Task Test_AddFewPhotos()
    {
        var photos = await GeneratePhotosInRepository(user.NonNullableId(), true);

        var userPhotos = await photosRepository.ReadAllFromUser(user.NonNullableId());
        Assert.AreEqual(photos.Length, userPhotos.Length);

        foreach (var (photoId, expectedPhotoBytes) in photos)
        {
            var actualPhotoBytes = await photosService.GetPhotoBytes(photoId);
            Assert.AreEqual(expectedPhotoBytes, actualPhotoBytes);
        }
    }

    [Test]
    public async Task Test_AnotherUserShouldSeeFeedAfterBecomingMutualFriendsWithUser()
    {
        var photos = await GeneratePhotosInRepository(user.NonNullableId());

        var anotherUser = UserGenerator.Generate();
        await usersService.CreateOrUpdate(anotherUser);

        var feed1 = await photosService.GetFeedForUser(anotherUser.NonNullableId());
        Assert.IsEmpty(feed1);

        // другой челик добавил меня в друзья, но все еще не видит в своей ленте мои фотки
        await usersService.TryAddFriend(anotherUser.NonNullableId(), user.NonNullableId());
        var feed2 = await photosService.GetFeedForUser(anotherUser.NonNullableId());
        Assert.IsEmpty(feed2);

        // я принял заявку челика в друзья, и теперь он видит мои фотки в своей ленте
        await usersService.TryAddFriend(user.NonNullableId(), anotherUser.NonNullableId());
        var feed3 = await photosService.GetFeedForUser(anotherUser.NonNullableId());
        Assert.IsNotEmpty(feed3);

        // челик решил пролайкать все мои фотки
        await CheckPhotosWithReactions(anotherUser, photos.Select(x => x.PhotoId), ReactionType.Like);
    }

    [Test]
    public async Task Test_UserShouldSeeAllPhotosFromFriends()
    {
        var friend1 = await CreateUserWithPhotosInRepository();
        var friend2 = await CreateUserWithPhotosInRepository();
        var friend3 = await CreateUserWithPhotosInRepository();

        // нет друзей - нет фоток
        var feed1 = await photosService.GetFeedForUser(user.NonNullableId());
        Assert.IsEmpty(feed1);

        // первый друг добавил нас в ответ
        await usersService.TryAddFriend(friend1.User.NonNullableId(), user.NonNullableId());
        var feed2 = await photosService.GetFeedForUser(user.NonNullableId(), 0, 20);
        Assert.AreEqual(friend1.PhotoIds.Length, feed2.Length);

        // второй друг добавил нас в ответ
        await usersService.TryAddFriend(friend2.User.NonNullableId(), user.NonNullableId());
        var feed3 = await photosService.GetFeedForUser(user.NonNullableId(), 0, 40);
        Assert.AreEqual(friend1.PhotoIds.Length + friend2.PhotoIds.Length, feed3.Length);

        // третий друг добавил нас в ответ
        await usersService.TryAddFriend(friend3.User.NonNullableId(), user.NonNullableId());
        var feed4 = await photosService.GetFeedForUser(user.NonNullableId(), 0, 60);
        Assert.AreEqual(friend1.PhotoIds.Length + friend2.PhotoIds.Length + friend3.PhotoIds.Length, feed4.Length);
        
        // первому и второму друзьям я ставлю лайки, а третьего считаю клоуном xdd
        await CheckPhotosWithReactions(user, friend1.PhotoIds, ReactionType.Like);
        await CheckPhotosWithReactions(user, friend2.PhotoIds, ReactionType.Like);
        await CheckPhotosWithReactions(user, friend3.PhotoIds, ReactionType.Clown);
    }

    private async Task<(Guid PhotoId, byte[] PhotoBytes)[]> GeneratePhotosInRepository(Guid userId, bool alsoCreateFiles = false)
    {
        var photos = Enumerable
            .Range(0, Randomizer.GenerateNumber(1, 20))
            .Select(_ => PhotoGenerator.Generate())
            .ToArray();

        var addPhotosTasks = photos.Select(x => photosService.CreatePhoto(new Photo
        {
            Id = x.PhotoId,
            UserId = userId
        }));
        await Task.WhenAll(addPhotosTasks);

        if (!alsoCreateFiles)
        {
            return photos;
        }

        var addPhotosBytesTasks = photos.Select(x => photosService.CreatePhotoFile(x.PhotoId, x.PhotoBytes));
        await Task.WhenAll(addPhotosBytesTasks);

        return photos;
    }

    private async Task<UserWithPhotos> CreateUserWithPhotosInRepository(bool alsoCreateFiles = false)
    {
        var friend = UserGenerator.Generate();
        await usersService.CreateOrUpdate(friend);
        await usersService.TryAddFriend(user.NonNullableId(), friend.NonNullableId());
        var photos = await GeneratePhotosInRepository(friend.NonNullableId(), alsoCreateFiles);

        return new UserWithPhotos
        {
            User = friend,
            PhotoIds = photos.Select(x => x.PhotoId).ToArray(),
            PhotoBytes = photos.Select(x => x.PhotoBytes).ToArray()
        };
    }

    private async Task CheckPhotosWithReactions(User reactedUser, IEnumerable<Guid> photos, ReactionType reactionType)
    {
        foreach (var id in photos)
        {
            await photosService.CreateReaction(new Reaction
            {
                PhotoId = id,
                UserId = reactedUser.NonNullableId(),
                ReactionType = reactionType
            });
            
            var reactions = await photosService.GetPhotoReactions(id);
            Assert.IsNotEmpty(reactions);
            Assert.NotNull(reactions[0].Id, "Автоматически должен создаваться айдишник реакции");
            Assert.AreEqual(id, reactions[0].PhotoId);
            Assert.AreEqual(reactedUser.NonNullableId(), reactions[0].UserId);
            Assert.AreEqual(reactionType, reactions[0].ReactionType);
        }
    }

    /// <summary>
    /// удаляем тестовую директорию с байтами фоток; игнорим, если она не создана (в тестах, где наличие файлов неважно)
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        try
        {
            Directory.Delete("PhotosForTest", true);
        }
        catch
        {
            // ignored
        }
    }

    private class UserWithPhotos
    {
        public User User { get; set; }
        public Guid[] PhotoIds { get; set; }
        public byte[][] PhotoBytes { get; set; }
    }

    private User user = null!;
    private IUsersService usersService = null!;
    private IPhotosService photosService = null!;
    private IPhotosRepository photosRepository = null!;
}