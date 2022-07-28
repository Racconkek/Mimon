using System.ComponentModel.DataAnnotations;

namespace Mimon.BusinessLogic.Repositories.Photos;

public class PhotoStorageElement
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
}