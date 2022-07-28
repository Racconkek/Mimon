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