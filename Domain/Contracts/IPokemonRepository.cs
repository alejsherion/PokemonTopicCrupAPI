using WebAPICrudPokemon.Models;

namespace WebAPICrudPokemon.Domain.Contracts;

public interface IPokemonRepository
{
    Task<IEnumerable<Pokemon>> GetPokemonsAsync();

    Task<Pokemon> GetAsync(Guid id);

    Task<Pokemon> GetByNameAsync(string name);

    Task AddAsync(Pokemon pokemon);

    Task UpdateAsync(Pokemon pokemon);

    Task RemoveAsync(Pokemon pokemon);

    Task RemoveAllAsync(string user);
}
