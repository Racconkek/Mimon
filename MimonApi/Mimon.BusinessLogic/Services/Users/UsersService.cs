using Mimon.Api.Dto.Users;
using Mimon.BusinessLogic.Repositories.Users;
using Mimon.BusinessLogic.Repositories.UsersRelations;

namespace Mimon.BusinessLogic.Services.Users;

public class UsersService : IUsersService
{
    public UsersService(
        IUsersRepository usersRepository,
        IRelationsRepository relationsRepository
    )
    {
        this.usersRepository = usersRepository;
        this.relationsRepository = relationsRepository;
    }

    public async Task CreateOrUpdate(User user)
    {
        await usersRepository.CreateOrUpdate(user);
    }

    public async Task<User> Find(Guid id)
    {
        return await usersRepository.Read(id);
    }

    public async Task<User[]> Find(Guid[] ids)
    {
        return await usersRepository.ReadMany(ids);
    }

    public async Task<UserFriend[]> FindUserFriends(Guid id, bool onlyMutual = false)
    {
        var ids = await relationsRepository.FindAll(id);
        var friends = await usersRepository.ReadMany(ids);
        var mutualTasks = friends.Select(x => relationsRepository.IsRelationExist(x.Id!.Value, id)).ToArray();
        var mutual = await Task.WhenAll(mutualTasks);
        return friends.Select((x, i) => new UserFriend
            {
                Friend = x,
                IsMutual = mutual[i]
            })
            .Where(x => !onlyMutual || x.IsMutual)
            .ToArray();
    }

    public async Task<bool> TryAddFriend(Guid userId, Guid friendId)
    {
        var isExist = await relationsRepository.IsRelationExist(userId, friendId);
        if (isExist)
        {
            return false;
        }

        await relationsRepository.Create(userId, friendId);
        return true;
    }

    private readonly IUsersRepository usersRepository;
    private readonly IRelationsRepository relationsRepository;
}