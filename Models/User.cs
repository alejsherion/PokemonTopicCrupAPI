using System.ComponentModel.DataAnnotations;

namespace WebAPICrudPokemon.Models
{
    public record User
    {
        public Guid Id { get; init; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreateAt { get; init; }
    }
}
