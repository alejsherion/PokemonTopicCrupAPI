using WebAPICrudPokemon.Models;

namespace WebAPICrudPokemon.Domain.Contracts;

public interface ILikeRepository
{
    Task<Like> GetLikeAsync(Guid id);

    Task<Like> GetLikeByPokemonIdAndUser(Guid pokemonId, string user);

    Task<IEnumerable<Like>> GetLikesAsync(string user);
    
    Task SaveLikedAsync(Like like);

    Task RemoveLikeAsync(Like like);
    
    Task RemoveAllLikeAsync(string user);
}
