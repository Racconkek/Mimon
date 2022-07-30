using Microsoft.EntityFrameworkCore;
using Mimon.Api.Dto.Users;
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
        var existingUser = await database.UsersStorage
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        if(existingUser is null)
        {
            await database.UsersStorage.AddAsync(ToStorageElement(user));
        }
        else
        {
            UpdateStorageElement(existingUser, user);
        }
        await database.SaveChangesAsync();
    }

    public async Task<User> Read(Guid id)
    {
        var result = await database.UsersStorage
            .FirstOrDefaultAsync(x => x.Id == id);

        return result != null ? ToModel(result) : throw new Exception("User doesn't exist");
    }

    public async Task<User[]> ReadMany(Guid[] ids)
    {
        var results = await database.UsersStorage
            .Where(x => ids.Contains(x.Id))
            .ToArrayAsync();

        return results
            .Select(ToModel)
            .ToArray();
    }

    private static User ToModel(UserStorageElement storageElement)
    {
        return new User 
            {
                Id = storageElement.Id,
                UserName = storageElement.UserName
            };
    }

    private static UserStorageElement ToStorageElement(User user)
    {
        return UpdateStorageElement(new UserStorageElement(), user);
    }

    private static UserStorageElement UpdateStorageElement(UserStorageElement existing, User user)
    {
        existing.Id = user.Id ?? Guid.NewGuid();
        existing.UserName = user.UserName;

        return existing;
    }

    private readonly DatabaseContext database;
}