using Mimon.Api.Dto;
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
        return await usersRepository.ReadWithOrderAndNulls(ids);
    }

    public async Task<User[]> FindUserFriends(Guid id)
    {
        var ids = await relationsRepository.FindFriends(id);
        return await usersRepository.ReadWithOrderAndNulls(ids);
    }

    private readonly IUsersRepository usersRepository;
    private readonly IRelationsRepository relationsRepository;
}