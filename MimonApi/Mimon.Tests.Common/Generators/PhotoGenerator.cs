using Mimon.Api.Dto.Photos;

namespace Mimon.Tests.Common.Generators;

public static class PhotoGenerator
{
    public static (Guid PhotoId, byte[] PhotoBytes) Generate()
    {
        var photoId = Guid.NewGuid();
        var bytes = Enumerable
            .Range(0, Randomizer.GenerateNumber(0, 100))
            .Select(_ => (byte)Randomizer.GenerateNumber(0, 256))
            .ToArray();

        return (photoId, bytes);
    }
}