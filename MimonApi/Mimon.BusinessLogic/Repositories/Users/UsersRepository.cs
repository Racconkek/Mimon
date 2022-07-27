using Microsoft.EntityFrameworkCore;
using Mimon.Api.Dto;
using Mimon.BusinessLogic.Repositories.Database;

namespace Mimon.BusinessLogic.Repositories.Users;

public class UsersRepository : IUsersRepository
{
    public UsersRepository(DatabaseContext database)
    {
        this.database = database;
    }

    public async Task CreateOrUpdate(User user)
    {
        await database.UsersStorage.AddAsync(ToStorageElement(user));
        await database.SaveChangesAsync();
    }

    public async Task<User> Read(Guid id)
    {
        var result = await database.UsersStorage
            .FindAsync(id);
        
        return ToModel(result) ?? throw new InvalidOperationException("User not found");
    }

    public async Task<User?[]> ReadWithOrderAndNulls(Guid[] ids)
    {
        var results = await database.UsersStorage
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id);

        return ids
            .Select(id => results.TryGetValue(id, out var user) ? user : null)
            .Select(ToModel)
            .ToArray();
    }

    private static User? ToModel(UserStorageElement? storageElement)
    {
        return storageElement == null
            ? null
            : new User 
            {
                Id = storageElement.Id,
                UserName = storageElement.UserName
            };
    }

    private static UserStorageElement ToStorageElement(User user)
    {
        return new UserStorageElement
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }

    private readonly DatabaseContext database;
}