using Mimon.Api.Dto;
using Mimon.Api.Dto.Photos;

namespace Mimon.BusinessLogic.Repositories.Reactions;

public interface IReactionsRepository
{
    Task Create(Reaction reaction);
    Task<Reaction[]> ReadAll(Guid photoId);
    Task<bool> TryDelete(Reaction reaction);
}