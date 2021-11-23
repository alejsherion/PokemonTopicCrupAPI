using Newtonsoft.Json;
using WebAPICrudPokemon.Application.Contracts;
using WebAPICrudPokemon.Domain.Contracts;
using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.Application;

public class PokemonAppService : IPokemonAppService
{
    private readonly HttpClient client;

    private readonly IPokemonRepository pokemonRepository;
    private readonly RequestHandler requestHandler;

    public PokemonAppService(IPokemonRepository _pokemonRepository, RequestHandler _requestHandler)
    {
        pokemonRepository = _pokemonRepository ?? throw new ArgumentNullException(nameof(IPokemonRepository));
        requestHandler = _requestHandler;

        client = new HttpClient();}

    public async Task<ResponseResult<IEnumerable<PokemonDTO>>> GetPokemonsAsync()
    {
        try
        {
            var pokemons = (await pokemonRepository.GetPokemonsAsync());
            var currentUser = requestHandler.GetCurrentUser();

            var pokemonsResult = pokemons
                .Where(p => p.CreateBy == "Public" || p.CreateBy == currentUser)
                .Select(pokemon => pokemon.ToDTO());

            return ResponseResult<IEnumerable<PokemonDTO>>.SetSuccessfully(pokemonsResult); ;
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
            var pokemon = await pokemonRepository.GetAsync(id);
            if (pokemon is not null)
            {
                var currentUser = requestHandler.GetCurrentUser();

                if (pokemon.CreateBy != currentUser && pokemon.CreateBy != "Public")
                    return ResponseResult<PokemonDTO>.SetUnSuccessfully("User can't get information of the pokemon that are not his own");
            }

            return ResponseResult<PokemonDTO>.SetSuccessfully(pokemon.ToDTO());
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
            var pokemon = await pokemonRepository.GetByNameAsync(name);
            if (pokemon is not null)
            {
                var currentUser = requestHandler.GetCurrentUser();

                if (pokemon.CreateBy != currentUser && pokemon.CreateBy != "Public")
                    return ResponseResult<PokemonDTO>.SetUnSuccessfully("User can't get information of the pokemon that are not his own");
            }

            return ResponseResult<PokemonDTO>.SetSuccessfully(pokemon.ToDTO()); ;
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
            var currentUser = requestHandler.GetCurrentUser();
            savedPokemon.CreateBy = currentUser;
            
            await pokemonRepository.AddAsync(savedPokemon);

            return ResponseResult<PokemonDTO>.SetSuccessfully(savedPokemon.ToDTO());
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
                return ResponseResult<PokemonDTO>.SetUnSuccessfully("Pokemon unexist");

            var currentUser = requestHandler.GetCurrentUser();
            if (existPokemon.CreateBy != currentUser)
                return ResponseResult<PokemonDTO>.SetUnSuccessfully("User can't modify the pokemon that are not his own");

            await pokemonRepository.UpdateAsync(pokemon.ToEntity());

            return ResponseResult<PokemonDTO>.SetSuccessfully();
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
                return ResponseResult<PokemonDTO>.SetUnSuccessfully("Pokemon unexist");

            var currentUser = requestHandler.GetCurrentUser();
            if (existPokemon.CreateBy != currentUser)
                return ResponseResult<PokemonDTO>.SetUnSuccessfully("User can't remove the pokemon that are not his own");

            await pokemonRepository.RemoveAsync(existPokemon);

            return ResponseResult<PokemonDTO>.SetSuccessfully();
        }
        catch (Exception ex)
        {
            return ResponseResult<PokemonDTO>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<PokemonDTO>> RemoveAllOwnAsync()
    {
        try
        {
            var currentUser = requestHandler.GetCurrentUser();
            
            await pokemonRepository.RemoveAllAsync(currentUser);
            
            return ResponseResult<PokemonDTO>.SetSuccessfully();
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

                return ResponseResult<PokemonResultAPIDTO>.SetSuccessfully(JsonConvert.DeserializeObject<PokemonResultAPIDTO>(serviceResponse));
            }
        }
        catch (Exception ex)
        {
            return ResponseResult<PokemonResultAPIDTO>.SetError(ex.Message);
        }
    }

}
