namespace Mimon.BusinessLogic.Repositories.Photos;

public class PhotoDataRepository : IPhotoDataRepository
{
    public PhotoDataRepository(bool testMode = false)
    {
        photosDirectory = testMode ? "PhotosForTest" : "Photos";
    }
    
    public async Task<byte[]> Read(Guid photoId)
    {
        return await File.ReadAllBytesAsync($"{photosDirectory}/{photoId}");
    }

    public async Task Write(Guid photoId, byte[] data)
    {
        if (!Directory.Exists(photosDirectory))
        {
            Directory.CreateDirectory(photosDirectory);
        }
        await File.WriteAllBytesAsync($"{photosDirectory}/{photoId}", data);
    }

    private readonly string photosDirectory;
}