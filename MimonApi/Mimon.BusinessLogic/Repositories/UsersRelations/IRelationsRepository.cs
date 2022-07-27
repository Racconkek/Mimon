namespace Mimon.BusinessLogic.Repositories.UsersRelations;

public interface IRelationsRepository
{
    Task CreateRelation(Guid userId, Guid friendId);
    Task<Guid[]> FindFriends(Guid userId);
}