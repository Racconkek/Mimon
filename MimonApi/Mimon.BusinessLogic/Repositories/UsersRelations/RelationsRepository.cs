using Microsoft.EntityFrameworkCore;
using Mimon.Api.Dto;
using Mimon.BusinessLogic.Repositories.Database;

namespace Mimon.BusinessLogic.Repositories.UsersRelations;

public class RelationsRepository : IRelationsRepository
{
    public RelationsRepository(DatabaseContext database)
    {
        this.database = database;
    }
    
    public async Task Create(Guid userId, Guid friendId)
    {
        await database.RelationsStorage.AddAsync(new RelationStorageElement
        {
            UserId = userId,
            FriendId = friendId
        });

        await database.SaveChangesAsync();
    }

    public async Task<Guid[]> FindAll(Guid userId)
    {
        var result = await database.RelationsStorage
            .Where(x => x.UserId == userId)
            .ToArrayAsync();

        return result.Select(x => x.FriendId).ToArray();
    }

    public async Task<bool> IsRelationExist(Guid userId, Guid friendId)
    {
        var existing = await database.RelationsStorage
            .FirstOrDefaultAsync(x => x.UserId == userId && x.FriendId == friendId);

        return existing != null;
    }

    private readonly DatabaseContext database;
}