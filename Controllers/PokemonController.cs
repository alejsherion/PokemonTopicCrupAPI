using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPICrudPokemon.Application;
using WebAPICrudPokemon.DTO;

namespace WebAPICrudPokemon.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class PokemonController : ControllerBase
{
    private readonly IPokemonAppService service;

    public PokemonController(IPokemonAppService _service)
        => service = _service;

    // GET / Pokemons
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PokemonDTO>>> GetAsync()
    {
        
        var user = HttpContext.User.FindFirstValue(ClaimTypes.Email);

        var result = await service.GetPokemonsAsync();
        if (result.IsError)
            return BadRequest(result.Message);

        return Ok(result.Result);
    }

    // GET / pokemons / id
    [HttpGet("{id}")]
    public async Task<ActionResult<PokemonDTO>> GetByIdAsync(Guid id)
    {
        var result = await service.GetAsync(id);
        if (result.IsError)
            return BadRequest(result.Message);

        return Ok(result.Result);
    }

    // POST / pokemons
    [HttpPost]
    public async Task<ActionResult> Add(CreatePokemonDTO pokemonDTO)
    {
        var result = await service.AddAsync(pokemonDTO);
        if (result.IsError)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetByIdAsync), new { _id = result.Result.Id },result.Result);
    }

    // PUT / pokemons / {id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, UpdatePokemonDTO pokemonDTO)
    {
        var resul = await service.UpdateAsync(id, pokemonDTO);
        if (resul.IsError)
            return BadRequest(resul.Message);
        if (!resul.IsSuccessful)
            return NotFound();

        return Ok(resul.Result);
    }

    // DELETE / pokemons / {id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveAsync(Guid id)
    {
        var resul = await service.RemoveAsync(id);
        if (resul.IsError)
            return BadRequest(resul.Message);
        if (!resul.IsSuccessful)
            return NotFound();

        return Ok(resul.Result);
    }

    // GET / GetInfoPokeAPIAsync / pokemonName
    [AllowAnonymous]
    [HttpGet]
    [Route("GetInfoPokeAPIAsync")]
    public async Task<ActionResult> GetInfoPokeAPIAsync(string pokemonName)
    {
        var result = await service.GetInfoPokeAPIAsync(pokemonName);
        if (result.IsError || !result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Result);
    }
}
