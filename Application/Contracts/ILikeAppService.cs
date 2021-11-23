using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.Application.Contracts;

public interface ILikeAppService
{
    Task<ResponseResult<IEnumerable<LikeDTO>>> GetLikesAsync();

    Task<ResponseResult<LikeDTO>> LikedAsync(CreateLikeDTO like);

    Task<ResponseResult<LikeDTO>> RemoveLikeAsync(Guid id);

    Task<ResponseResult<LikeDTO>> RemoveAllLikeAsync();
}
