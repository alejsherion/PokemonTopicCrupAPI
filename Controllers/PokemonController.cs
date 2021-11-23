using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPICrudPokemon.Application.Contracts;
using WebAPICrudPokemon.DTO;

namespace WebAPICrudPokemon.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class PokemonController : ControllerBase
{
    private readonly IPokemonAppService service;

    public PokemonController(IPokemonAppService _service)
        => service = _service ?? throw new ArgumentNullException(nameof(IPokemonAppService));

    // GET / Pokemons
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PokemonDTO>>> GetAsync()
    {
        var result = await service.GetPokemonsAsync();
        if (result.IsError || !result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Result);
    }

    // GET / pokemons / id
    [HttpGet("{id}")]
    public async Task<ActionResult<PokemonDTO>> GetByIdAsync(Guid id)
    {
        var result = await service.GetAsync(id);
        if (result.IsError || !result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Result);
    }

    // POST / pokemons
    [HttpPost]
    public async Task<ActionResult> Add(CreatePokemonDTO pokemonDTO)
    {
        var result = await service.AddAsync(pokemonDTO);
        if (result.IsError || !result.IsSuccessful)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetByIdAsync), new { _id = result.Result.Id }, result.Result);
    }

    // PUT / pokemons / {id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, UpdatePokemonDTO pokemonDTO)
    {
        var result = await service.UpdateAsync(id, pokemonDTO);
        if (result.IsError)
            return BadRequest(result.Message);
        if (!result.IsSuccessful)
            return NotFound(result.Message);

        return Ok(result.Result);
    }

    // DELETE / pokemons / {id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveAsync(Guid id)
    {
        var result = await service.RemoveAsync(id);
        if (result.IsError)
            return BadRequest(result.Message);
        if (!result.IsSuccessful)
            return NotFound(result.Message);

        return Ok();
    }

    // DELETE / pokemons
    [HttpDelete()]
    [Route("RemoveAllOwn")]
    public async Task<ActionResult> RemoveAllOwnAsync()
    {
        var result = await service.RemoveAllOwnAsync();
        if (result.IsError)
            return BadRequest(result.Message);
        if (!result.IsSuccessful)
            return NotFound(result.Message);

        return Ok();
    }

    // GET / GetInfoPokeAPIAsync / pokemonName
    [AllowAnonymous]
    [HttpGet]
    [Route("GetInfoPokeAPI")]
    public async Task<ActionResult> GetInfoPokeAPIAsync(string pokemonName)
    {
        var result = await service.GetInfoPokeAPIAsync(pokemonName);
        if (result.IsError || !result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Result);
    }
}
