using System.ComponentModel.DataAnnotations;
using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.DTO;

public record CreatePokemonDTO
{
    [Required]
    [Range(1, 898)]
    public int PokedexOrder { get; init; }
    [Required]
    public string Name { get; init; }
    [Required]
    public string Region { get; init; }
    [Required]
    public string Type { get; init; }
}
