namespace WebAPICrudPokemon.DTO;

public record PokemonResultAPIDTO
{
    public int Id { get; init; }
    public int Height { get; init; }
    public int Weight { get; init; }
    public string Location_area_enconunters { get; init; }
    public string Name { get; init; }
    public int Order { get; init; }
    public PokemonResultAPIDTOSprites Sprites { get; init; }
}

public record PokemonResultAPIDTOSprites
{
    public string Back_default { get; init; }
    public string Back_female { get; init; }
    public string Back_shiny { get; init; }
    public string Back_shiny_female { get; init; }
    public string Front_default { get; init; }
    public string Front_female { get; init; }
    public string Front_shiny { get; init; }
    public string Front_shiny_female { get; init; }
}
