using Microsoft.AspNetCore.Mvc;
using WebAPICrudPokemon.Application.Contracts;
using WebAPICrudPokemon.DTO;

namespace WebAPICrudPokemon.Controllers;

[ApiController]
[Route("[Controller]")]
public class AthenticationController : ControllerBase
{
    private readonly IAuthenticationAppService service;

    public AthenticationController(IAuthenticationAppService _service)
        => service = _service;

    [HttpPost]
    [Route("SignIn")]
    public async Task<ActionResult> SignIn(UserDTO user)
    {
        var result = await service.SignIn(user);
        if (result.IsError || !result.IsSuccessful)
            return BadRequest(result.Message);
        
        return Ok(result.Result);
    }

    [HttpPost]
    [Route("SignUp")]
    public async Task<ActionResult> SignUp(UserDTO user)
    {
        var result = await service.SignUp(user);
        if (result.IsError)
            return BadRequest(result.Message);

        return Ok("User created successfully!");
    }
}
