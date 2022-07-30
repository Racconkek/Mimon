namespace Mimon.BusinessLogic.Repositories.Photos;

public interface IPhotoDataRepository
{
    Task<byte[]> Read(Guid photoId);
    Task Write(Guid photoId, byte[] data);
}