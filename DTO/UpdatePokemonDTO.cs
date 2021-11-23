using System.ComponentModel.DataAnnotations;

namespace WebAPICrudPokemon.DTO;

public class UpdatePokemonDTO
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
