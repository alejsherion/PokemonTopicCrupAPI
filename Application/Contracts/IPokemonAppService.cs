using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.Application.Contracts;

public interface IPokemonAppService
{
    Task<ResponseResult<PagintaionResultDTO<PokemonDTO>>> GetPokemonsAsync();

    Task<ResponseResult<PokemonDTO>> GetAsync(Guid id);

    Task<ResponseResult<PokemonDTO>> GetByNameAsync(string name);

    Task<ResponseResult<PokemonDTO>> AddAsync(CreatePokemonDTO pokemon);

    Task<ResponseResult<PokemonDTO>> UpdateAsync(Guid id, UpdatePokemonDTO pokemon);

    Task<ResponseResult<PokemonDTO>> RemoveAsync(Guid id);

    Task<ResponseResult<PokemonDTO>> RemoveAllOwnAsync();

    Task<ResponseResult<PokemonResultAPIDTO>> GetInfoPokeAPIAsync(string pokemonName);
}
