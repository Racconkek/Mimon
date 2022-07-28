namespace Mimon.BusinessLogic.Repositories.UsersRelations;

public class FakeRelationsRepository : IRelationsRepository
{
    public FakeRelationsRepository()
    {
        storage = new List<Tuple<Guid, Guid>>();
    }
    
    public Task Create(Guid userId, Guid friendId)
    {
        storage.Add(new Tuple<Guid, Guid>(userId, friendId));
        
        return Task.CompletedTask;
    }

    public Task<Guid[]> FindAll(Guid userId)
    {
        var result = storage
            .Where(x => x.Item1 == userId)
            .Select(x => x.Item2)
            .ToArray();

        return Task.FromResult(result);
    }

    public Task<bool> IsRelationExist(Guid userId, Guid friendId)
    {
        var result = storage.Any(x => x.Item1 == userId && x.Item2 == friendId);

        return Task.FromResult(result);
    }

    private readonly List<Tuple<Guid, Guid>> storage;
}