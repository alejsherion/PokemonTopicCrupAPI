using WebAPICrudPokemon.Domain.Contracts;
using WebAPICrudPokemon.Helper;
using WebAPICrudPokemon.Models;

namespace WebAPICrudPokemon.Domain;

public class InMemoryPokemonRepository : IPokemonRepository
{
    private readonly List<Pokemon> InMemoryPokemons = new()
    {
        new Pokemon() { Id = Guid.NewGuid(), Name = "Charmander", PokedexOrder = 4, Region = "Kanto", Type = "Fire", CreateAt = DateTime.Now },
        new Pokemon() { Id = Guid.NewGuid(), Name = "Bagon", PokedexOrder = 371, Region = "Hoen", Type = "Dragon", CreateAt = DateTime.Now },
        new Pokemon() { Id = Guid.NewGuid(), Name = "Totodile", PokedexOrder = 158, Region = "Johto", Type = "Water", CreateAt = DateTime.Now },
        new Pokemon() { Id = Guid.NewGuid(), Name = "Ditto", PokedexOrder = 132, Region = "Kanto", Type = "Normal", CreateAt = DateTime.Now },
        new Pokemon() { Id = Guid.NewGuid(), Name = "Zoroark", PokedexOrder = 571, Region = "Kalos", Type = "Sinister", CreateAt = DateTime.Now },
        new Pokemon() { Id = Guid.NewGuid(), Name = "Zeraora", PokedexOrder = 807, Region = "Teselia", Type = "Electric", CreateAt = DateTime.Now },
    };

    public async Task<IEnumerable<Pokemon>> GetPokemonsAsync()
        => await Task.FromResult(InMemoryPokemons);

    public async Task<Pokemon> GetAsync(Guid id)
        => await Task.FromResult(InMemoryPokemons.FirstOrDefault(p => p.Id == id));

    public async Task<Pokemon> GetByNameAsync(string name)
        => await Task.FromResult(InMemoryPokemons.FirstOrDefault(p => p.Name == name));

    public async Task AddAsync(Pokemon pokemon)
    {
        InMemoryPokemons.Add(pokemon);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Pokemon pokemon)
    {
        var index = InMemoryPokemons.FindIndex(existing => existing.Id == pokemon.Id);
        InMemoryPokemons[index] = pokemon;
        await Task.CompletedTask;
    }

    public async Task RemoveAsync(Pokemon pokemon)
    {
        InMemoryPokemons.Remove(pokemon);
        await Task.CompletedTask;
    }

    public Task RemoveAllAsync(string user) => throw new NotImplementedException();
}
