using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPICrudPokemon.Application.Contracts;
using WebAPICrudPokemon.DTO;

namespace WebAPICrudPokemon.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class LikeController : Controller
{
    private readonly ILikeAppService service;

    public LikeController(ILikeAppService _service)
        => service = _service ?? throw new ArgumentNullException(nameof(ILikeAppService));

    // GET / Like
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDTO>>> GetLikesAsync()
    {
        var result = await service.GetLikesAsync();
        if (result.IsError || !result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Result);
    }

    // POST / Like
    [HttpPost]
    public async Task<ActionResult<LikeDTO>> LikedAsync(CreateLikeDTO like)
    {
        var result = await service.LikedAsync(like);
        if (result.IsError || !result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Result);
    }

    // DELETE / Like / {id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveLikeAsync(Guid id)
    {
        var result = await service.RemoveLikeAsync(id);
        if (result.IsError)
            return BadRequest(result.Message);
        if (!result.IsSuccessful)
            return NotFound(result.Message);

        return Ok();
    }

    // DELETE / Like
    [HttpDelete]
    [Route("RemoveAllLike")]
    public async Task<ActionResult> RemoveAllLikeAsync()
    {
        var result = await service.RemoveAllLikeAsync();
        if (result.IsError)
            return BadRequest(result.Message);
        if (!result.IsSuccessful)
            return NotFound(result.Message);

        return Ok();
    }
}