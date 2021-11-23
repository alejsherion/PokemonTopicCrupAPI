using Newtonsoft.Json;
using WebAPICrudPokemon.Domain;
using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.Application;

public class PokemonAppService : IPokemonAppService
{
    private readonly HttpClient client;

    private readonly IPokemonRepository pokemonRepository;

    public PokemonAppService(IPokemonRepository _pokemonRepository)
    {
        pokemonRepository = _pokemonRepository;

        client = new HttpClient();
    }

    public async Task<ResponseResult<IEnumerable<PokemonDTO>>> GetPokemonsAsync()
    {
        try
        {
            var pokemons = (await pokemonRepository.GetPokemonsAsync()).Select(pokemon => pokemon.ToDTO());
            
            return ResponseResult<IEnumerable<PokemonDTO>>.SetSuccess(pokemons); ;
        }
        catch (Exception ex)
        {
            return ResponseResult<IEnumerable<PokemonDTO>>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<PokemonDTO>> GetAsync(Guid id)
    {
        try
        {
            var pokemons = (await pokemonRepository.GetAsync(id)).ToDTO();

            return ResponseResult<PokemonDTO>.SetSuccess(pokemons); ;
        }
        catch (Exception ex)
        {
            return ResponseResult<PokemonDTO>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<PokemonDTO>> GetByNameAsync(string name)
    {
        try
        {
            var pokemons = (await pokemonRepository.GetByNameAsync(name)).ToDTO();

            return ResponseResult<PokemonDTO>.SetSuccess(pokemons); ;
        }
        catch (Exception ex)
        {
            return ResponseResult<PokemonDTO>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<PokemonDTO>> AddAsync(CreatePokemonDTO pokemon)
    {
        try
        {
            pokemon.IsNull();

            var savedPokemon = pokemon.ToEntity();
            await pokemonRepository.AddAsync(savedPokemon);

            return ResponseResult<PokemonDTO>.SetSuccess(savedPokemon.ToDTO());
        }
        catch (Exception ex)
        {
            return ResponseResult<PokemonDTO>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<PokemonDTO>> UpdateAsync(Guid id, UpdatePokemonDTO pokemon)
    {
        try
        {
            pokemon.IsNull();
            if (id == Guid.Empty)
                return ResponseResult<PokemonDTO>.SetError("Id can't be empty");

            var existPokemon = await pokemonRepository.GetAsync(id);
            if (existPokemon == null)
                return ResponseResult<PokemonDTO>.SetUnSuccess("Pokemon unexist");

            await pokemonRepository.UpdateAsync(pokemon.ToEntity());

            return ResponseResult<PokemonDTO>.SetSuccess(null);
        }
        catch (Exception ex)
        {
            return ResponseResult<PokemonDTO>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<PokemonDTO>> RemoveAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return ResponseResult<PokemonDTO>.SetError("Id can't be empty");

            var existPokemon = await pokemonRepository.GetAsync(id);
            if (existPokemon == null)
                return ResponseResult<PokemonDTO>.SetUnSuccess("Pokemon unexist");

            await pokemonRepository.RemoveAsync(existPokemon);

            return ResponseResult<PokemonDTO>.SetSuccess(null);
        }
        catch (Exception ex)
        {
            return ResponseResult<PokemonDTO>.SetError(ex.Message);
        }
    }
    
    public async Task<ResponseResult<PokemonResultAPIDTO>> GetInfoPokeAPIAsync(string pokemonName)
    {
        try
        {
            const string URL_POKE_API = "https://pokeapi.co/api/v2/pokemon/";

            using (HttpResponseMessage response = client.GetAsync($"{URL_POKE_API}/{pokemonName}").Result)
            using (HttpContent content = response.Content)
            {
                string serviceResponse = await content.ReadAsStringAsync();

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return ResponseResult<PokemonResultAPIDTO>.SetError(serviceResponse);

                return ResponseResult<PokemonResultAPIDTO>.SetSuccess(JsonConvert.DeserializeObject<PokemonResultAPIDTO>(serviceResponse));
            }
        }
        catch (Exception ex)
        {
            return ResponseResult<PokemonResultAPIDTO>.SetError(ex.Message);
        }
    } 
}
