using Microsoft.EntityFrameworkCore;
using Mimon.Api.Dto.Photos;
using Mimon.BusinessLogic.Repositories.Database;

namespace Mimon.BusinessLogic.Repositories.Reactions;

public class ReactionsRepository : IReactionsRepository
{
    private readonly DatabaseContext databaseContext;

    public ReactionsRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task Create(Reaction reaction)
    {
        await databaseContext.ReactionsStorage.AddAsync(ToStorageElement(reaction));
        await databaseContext.SaveChangesAsync();
    }

    public async Task<Reaction[]> ReadAll(Guid photoId)
    {
        var results = await databaseContext.ReactionsStorage.Where(x => x.PhotoId == photoId).ToArrayAsync();

        return results.Select(ToModel).ToArray();
    }

    public async Task<bool> TryDelete(Reaction reaction)
    {
        var element = await databaseContext.ReactionsStorage
            .FirstOrDefaultAsync(x => x.Id == reaction.Id
                                      && x.PhotoId == reaction.PhotoId
                                      && x.UserId == reaction.UserId
                                      && x.ReactionType == reaction.ReactionType);
        if (element is null)
        {
            return false;
        }

        databaseContext.ReactionsStorage.Remove(element);
        await databaseContext.SaveChangesAsync();
        return true;
    }

    private static Reaction ToModel(ReactionStorageElement storageElement)
    {
        return new Reaction
        {
            Id = storageElement.Id,
            PhotoId = storageElement.PhotoId,
            UserId = storageElement.UserId,
            ReactionType = storageElement.ReactionType
        };
    }

    private static ReactionStorageElement ToStorageElement(Reaction reaction)
    {
        return new ReactionStorageElement
        {
            Id = reaction.Id ?? Guid.NewGuid(),
            PhotoId = reaction.PhotoId,
            UserId = reaction.UserId,
            ReactionType = reaction.ReactionType
        };
    }
}