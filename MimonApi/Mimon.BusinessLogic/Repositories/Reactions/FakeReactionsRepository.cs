using Mimon.Api.Dto.Photos;

namespace Mimon.BusinessLogic.Repositories.Reactions;

public class FakeReactionsRepository : IReactionsRepository
{
    public FakeReactionsRepository()
    {
        storage = new Dictionary<Guid, List<Reaction>>();
    }

    public Task Create(Reaction reaction)
    {
        if (!storage.ContainsKey(reaction.PhotoId))
        {
            storage[reaction.PhotoId] = new List<Reaction>();
        }
        
        reaction.Id ??= Guid.NewGuid();
        storage[reaction.PhotoId].Add(reaction);
        return Task.CompletedTask;
    }

    public Task<Reaction[]> ReadAll(Guid photoId)
    {
        return Task.FromResult(storage[photoId].ToArray());
    }

    public Task<bool> TryDelete(Reaction reaction)
    {
        if (!storage.TryGetValue(reaction.PhotoId, out var photoReactions))
        {
            return Task.FromResult(false);
        }
        
        var matchingElement = photoReactions.FirstOrDefault(x => x.Id == reaction.Id);
        var result = matchingElement != null && photoReactions.Remove(matchingElement);
            
        return Task.FromResult(result);
    }

    private readonly Dictionary<Guid, List<Reaction>> storage;
}