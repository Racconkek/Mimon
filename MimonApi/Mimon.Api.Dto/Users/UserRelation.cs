namespace Mimon.Api.Dto.Users;

public class UserRelation
{
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
    public bool IsMutual { get; set; }
}