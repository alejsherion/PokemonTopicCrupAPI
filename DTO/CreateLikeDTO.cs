using System.ComponentModel.DataAnnotations;

namespace WebAPICrudPokemon.DTO
{
    public class CreateLikeDTO
    {
        [Required]
        public Guid PokemonId { get; set; }        
    }
}
