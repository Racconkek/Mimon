using Mimon.Api.Dto.Users;

namespace Mimon.BusinessLogic.Services.Users;

public interface IUsersService
{
    Task CreateOrUpdate(User user);
    Task<User> Find(Guid id);
    Task<User[]> Find(Guid[] ids);
    Task<UserFriend[]> FindUserFriends(Guid id, bool onlyMutual = false);
    Task<bool> TryAddFriend(Guid userId, Guid friendId);
}