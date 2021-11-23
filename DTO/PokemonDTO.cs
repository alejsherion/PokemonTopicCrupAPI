using System.ComponentModel.DataAnnotations;
using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.DTO;

public record PokemonDTO
{
    public Guid Id { get; init; }
    [Required]
    [Range(1, 898)]
    public int PokedexOrder { get; init; }
    [Required]
    public string Name { get; init; }
    [Required]
    public string Region { get; init; }
    [Required]
    public string Type { get; init; }
    public DateTimeOffset CreateAt { get; init; }
}
