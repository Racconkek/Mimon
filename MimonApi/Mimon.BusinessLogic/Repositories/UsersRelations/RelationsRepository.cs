namespace Mimon.BusinessLogic.Repositories.UsersRelations;

public class RelationsRepository : IRelationsRepository
{
    public RelationsRepository()
    {
        
    }
    
    public Task CreateRelation(Guid userId, Guid friendId)
    {
        throw new NotImplementedException();
    }

    public Task<Guid[]> FindFriends(Guid userId)
    {
        throw new NotImplementedException();
    }
}