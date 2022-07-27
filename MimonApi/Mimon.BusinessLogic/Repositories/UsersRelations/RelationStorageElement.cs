using System.ComponentModel.DataAnnotations;

namespace Mimon.BusinessLogic.Repositories.UsersRelations;

public class RelationStorageElement
{
    [Key]
    public Guid UserId { get; set; }
    [Key]
    public Guid FriendId { get; set; }
}