using Microsoft.AspNetCore.Mvc;
using Mimon.Api.Dto.Users;
using Mimon.BusinessLogic.Services.Users;

namespace Mimon.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    public UsersController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrUpdateUser([FromBody] User user)
    {
        await usersService.CreateOrUpdate(user);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> FindUser([FromRoute] Guid id)
    {
        try
        {
            return await usersService.Find(id);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost("find")]
    public async Task<ActionResult<User[]>> FindUsers([FromBody] Guid[] ids)
    {
        return await usersService.Find(ids);
    }

    [HttpGet("{id:guid}/friends")]
    public async Task<ActionResult<UserFriend[]>> GetUserFriends([FromRoute] Guid id)
    {
        return await usersService.FindUserFriends(id);
    }

    [HttpGet("{id:guid}/friends/add/{friendId:guid}")]
    public async Task<ActionResult> AddFriend([FromRoute] Guid id, [FromRoute] Guid friendId)
    {
        var addedSuccessfully = await usersService.TryAddFriend(id, friendId);
        return addedSuccessfully ? Ok() : Conflict("Users are already friends");
    }

    private readonly IUsersService usersService;
}