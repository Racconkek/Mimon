using Mimon.Api.Dto.Users;

namespace Mimon.Tests.Common.Extensions;

public static class UserExtensions
{
    public static Guid NonNullableId(this User user)
    {
        return user.Id!.Value;
    }
}