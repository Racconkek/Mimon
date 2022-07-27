using Mimon.Api.Dto;

namespace Mimon.BusinessLogic.Services.Users;

public interface IUsersService
{
    Task CreateOrUpdate(User user);
    Task<User> Find(Guid id);
    Task<User[]> Find(Guid[] ids);
    Task<User[]> FindUserFriends(Guid id);
}