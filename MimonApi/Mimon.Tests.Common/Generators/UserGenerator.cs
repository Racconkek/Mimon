using Mimon.Api.Dto.Users;

namespace Mimon.Tests.Common.Generators;

public class UserGenerator
{
    public static User Generate()
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserName = Randomizer.GenerateString(10)
        };
    }
}