namespace Mimon.BusinessLogic.Repositories.UsersRelations;

public interface IRelationsRepository
{
    Task Create(Guid userId, Guid friendId);
    Task<Guid[]> FindAll(Guid userId);
    Task<bool> IsRelationExist(Guid userId, Guid friendId);
}