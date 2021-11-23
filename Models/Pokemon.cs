using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.Models;

public record Pokemon
{
    public Guid Id { get; init; }
    public int PokedexOrder { get; init; }
    public string Name { get; init; }
    public string Region { get; init; }
    public string Type { get; init; }
    public string CreateBy { get; set; }
    public DateTimeOffset CreateAt { get; init; }
}
 