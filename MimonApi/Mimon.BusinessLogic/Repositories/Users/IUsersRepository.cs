using Mimon.Api.Dto;

namespace Mimon.BusinessLogic.Repositories.Users;

public interface IUsersRepository
{
    Task CreateOrUpdate(User user);
    Task<User> Read(Guid id);
    Task<User[]> ReadMany(Guid[] ids);
}