using Microsoft.AspNetCore.Mvc;
using Mimon.Api.Dto;
using Mimon.BusinessLogic.Services.Users;

namespace MimonAPI.Controllers;

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

    [HttpGet]
    public async Task<ActionResult<User[]>> FindUser([FromBody] Guid[] ids)
    {
        return await usersService.Find(ids);
    }

    [HttpPost]
    public async Task<ActionResult<User[]>> GetUserFriends([FromRoute] Guid id)
    {
        return await usersService.FindUserFriends(id);
    }

    private readonly IUsersService usersService;
}