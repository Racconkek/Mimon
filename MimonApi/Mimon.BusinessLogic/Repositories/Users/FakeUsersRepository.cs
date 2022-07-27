using Mimon.Api.Dto;

namespace Mimon.BusinessLogic.Repositories.Users;

public class FakeUsersRepository : IUsersRepository
{
    public FakeUsersRepository()
    {
        users = new Dictionary<Guid, User>();
    }
    
    public Task CreateOrUpdate(User user)
    {
        users[user.Id] = user;

        return Task.CompletedTask;
    }

    public Task<User> Read(Guid id)
    {
        return Task.FromResult(users[id]);
    }

    public Task<User?[]> ReadWithOrderAndNulls(Guid[] ids)
    {
        var result = ids.Select(id => users.TryGetValue(id, out var user) ? user : null).ToArray();

        return Task.FromResult(result);
    }

    private readonly Dictionary<Guid, User> users;
}