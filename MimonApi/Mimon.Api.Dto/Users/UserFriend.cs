namespace Mimon.Api.Dto.Users;

public class UserFriend
{
    public User Friend { get; set; }
    public bool IsMutual { get; set; }
}