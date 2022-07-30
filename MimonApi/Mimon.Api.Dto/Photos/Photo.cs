namespace Mimon.Api.Dto.Photos;

public class Photo
{
    public Guid? Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime? CreationDateTime { get; set; }
}