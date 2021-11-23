namespace WebAPICrudPokemon.Models;

public record Like
{
    public Guid Id { get; set; }
    public Guid PokemonId { get; set; }
    public string User { get; set; }
    public DateTimeOffset CreateAt { get; set; }
}
