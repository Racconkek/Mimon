using Mimon.Api.Dto.Photos;

namespace Mimon.Tests.Common.Extensions;

public static class PhotoExtensions
{
    public static Guid NonNullableId(this Photo photo)
    {
        return photo.Id!.Value;
    }
}