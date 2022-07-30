using Microsoft.AspNetCore.Mvc;
using Mimon.Api.Dto.Photos;
using Mimon.BusinessLogic.Services.Photos;

namespace Mimon.Api.Controllers;

[ApiController]
[Route("photos")]
public class PhotosController : ControllerBase
{
    public PhotosController(IPhotosService photosService)
    {
        this.photosService = photosService;
    }

    [HttpGet("{photoId:guid}/data")]
    public async Task<ActionResult<Photo>> GetPhoto([FromRoute] Guid photoId)
    {
        try
        {
            return await photosService.GetPhotoData(photoId);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpGet("{photoId:guid}/raw")]
    public async Task<ActionResult<byte[]>> GetRawPhoto([FromRoute] Guid photoId)
    {
        try
        {
            return await photosService.GetPhotoBytes(photoId);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePhoto([FromBody] Photo photo)
    {
        return await photosService.CreatePhoto(photo);
    }

    [HttpPost("{photoId:guid}")]
    public async Task<ActionResult> CreatePhotoFile([FromRoute] Guid photoId, [FromBody] byte[] bytes)
    {
        await photosService.CreatePhotoFile(photoId, bytes);
        return Ok();
    }

    [HttpGet("feed/{userId:guid}")]
    public async Task<ActionResult<Photo[]>> GetFeedForUser([FromRoute] Guid userId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        return await photosService.GetFeedForUser(userId);
    }

    [HttpGet("{photoId:guid}/reactions")]
    public async Task<ActionResult<Reaction[]>> GetPhotoReactions([FromRoute] Guid photoId)
    {
        return await photosService.GetPhotoReactions(photoId);
    }

    [HttpPost("{photoId:guid}/reactions")]
    public async Task<ActionResult> CreateReaction([FromRoute] Guid photoId, [FromBody] Reaction reaction)
    {
        await photosService.CreateReaction(reaction);
        return Ok();
    }

    [HttpDelete("{photoId:guid}/reactions")]
    public async Task<ActionResult<bool>> RemoveReaction([FromRoute] Guid photoId, [FromBody] Reaction reaction)
    {
        return await photosService.DeleteReaction(reaction);
    }

    private readonly IPhotosService photosService;
}