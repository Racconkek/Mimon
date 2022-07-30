using System.Linq;
using System.Threading.Tasks;
using Mimon.Api.Dto.Users;
using Mimon.BusinessLogic.Repositories.Users;
using Mimon.BusinessLogic.Repositories.UsersRelations;
using Mimon.BusinessLogic.Services.Users;
using Mimon.Tests.Common.Extensions;
using Mimon.Tests.Common.Generators;
using NUnit.Framework;

namespace Mimon.Tests.UnitTests;

public class UsersTests
{
    [SetUp]
    public void Setup()
    {
        usersService = new UsersService(new FakeUsersRepository(), new FakeRelationsRepository());
        user = UserGenerator.Generate();
        usersService.CreateOrUpdate(user).GetAwaiter().GetResult();
    }

    [Test]
    public async Task Test_CreateReadUpdateUser()
    {
        var createdUser = await usersService.Find(user.NonNullableId());
        Assert.AreEqual(user.Id, createdUser.Id);
        user.UserName = Randomizer.GenerateString(10);
        await usersService.CreateOrUpdate(user);
        var updatedUser = await usersService.Find(user.NonNullableId());
        Assert.AreEqual(user.UserName, updatedUser.UserName);
    }

    [Test]
    public async Task Test_Friends()
    {
        var anotherUser = UserGenerator.Generate();
        await usersService.CreateOrUpdate(anotherUser);
        await usersService.TryAddFriend(user.NonNullableId(), anotherUser.NonNullableId());

        var user1Friends = await usersService.FindUserFriends(user.NonNullableId());
        var user1FriendsOnlyMutual = await usersService.FindUserFriends(user.NonNullableId(), true);
        var user2Friends = await usersService.FindUserFriends(anotherUser.NonNullableId());

        Assert.IsNotEmpty(user1Friends);
        Assert.IsFalse(user1Friends[0].IsMutual);
        Assert.IsEmpty(user1FriendsOnlyMutual);
        Assert.IsEmpty(user2Friends);

        await usersService.TryAddFriend(anotherUser.NonNullableId(), user.NonNullableId());

        user1Friends = await usersService.FindUserFriends(user.NonNullableId());
        user1FriendsOnlyMutual = await usersService.FindUserFriends(user.NonNullableId(), true);
        user2Friends = await usersService.FindUserFriends(anotherUser.NonNullableId());
        var user2FriendsOnlyMutual = await usersService.FindUserFriends(anotherUser.NonNullableId(), true);

        Assert.IsNotEmpty(user1Friends);
        Assert.IsTrue(user1Friends[0].IsMutual);
        Assert.IsNotEmpty(user1FriendsOnlyMutual);
        Assert.IsNotEmpty(user2Friends);
        Assert.IsTrue(user2Friends[0].IsMutual);
        Assert.IsNotEmpty(user2FriendsOnlyMutual);
    }

    [Test]
    public async Task Test_MultipleFriends()
    {
        var friendsCount = Randomizer.GenerateNumber(0, 20);
        var friends = Enumerable
            .Range(0, friendsCount)
            .Select(_ => UserGenerator.Generate())
            .ToArray();

        var expectedMutual = 0;
        foreach (var friend in friends)
        {
            await usersService.CreateOrUpdate(friend);
            await usersService.TryAddFriend(user.NonNullableId(), friend.NonNullableId());
            if (Randomizer.Yep())
            {
                await usersService.TryAddFriend(friend.NonNullableId(), user.NonNullableId());
                expectedMutual++;
            }
        }

        var friendsResult = await usersService.FindUserFriends(user.NonNullableId());
        Assert.AreEqual(friendsCount, friendsResult.Length);
        var friendsResultOnlyMutual = await usersService.FindUserFriends(user.NonNullableId(), true);
        Assert.AreEqual(expectedMutual, friendsResultOnlyMutual.Length);
    }

    private User user = null!;
    private IUsersService usersService = null!;
}