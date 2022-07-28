using Mimon.Api.Dto;

namespace Mimon.BusinessLogic.Services.Users;

public interface IUsersService
{
    Task CreateOrUpdate(User user);
    Task<User> Find(Guid id);
    Task<User[]> Find(Guid[] ids);
    Task<UserFriend[]> FindUserFriends(Guid id);
    Task<bool> TryAddFriend(Guid userId, Guid friendId);
}