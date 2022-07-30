using Mimon.Api.Dto.Users;

namespace Mimon.BusinessLogic.Repositories.Users;

public class FakeUsersRepository : IUsersRepository
{
    public FakeUsersRepository()
    {
        users = new Dictionary<Guid, User>();
    }
    
    public Task CreateOrUpdate(User user)
    {
        user.Id ??= Guid.NewGuid();
        users[user.Id.Value] = user;

        return Task.CompletedTask;
    }

    public Task<User> Read(Guid id)
    {
        return Task.FromResult(users[id]);
    }

    public Task<User[]> ReadMany(Guid[] ids)
    {
        var result = ids
            .Where(id => users.ContainsKey(id))
            .Select(id => users[id])
            .ToArray();

        return Task.FromResult(result);
    }

    private readonly Dictionary<Guid, User> users;
}