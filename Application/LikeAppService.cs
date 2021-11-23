using WebAPICrudPokemon.Application.Contracts;
using WebAPICrudPokemon.Domain.Contracts;
using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Helper;
using WebAPICrudPokemon.Models;

namespace WebAPICrudPokemon.Application;

public class LikeAppService : ILikeAppService
{
    private readonly ILikeRepository likeRepository;
    private readonly IPokemonRepository pokemonRepository;

    private readonly RequestHandler requestHandler;

    public LikeAppService(ILikeRepository _likeRepository, IPokemonRepository _pokemonRepository, RequestHandler _requestHandler)
    {
        likeRepository = _likeRepository ?? throw new ArgumentNullException(nameof(ILikeRepository));
        pokemonRepository = _pokemonRepository ?? throw new ArgumentNullException(nameof(IPokemonRepository));

        requestHandler = _requestHandler;
    }

    public async Task<ResponseResult<PagintaionResultDTO<LikeDTO>>> GetLikesAsync()
    {
        try
        {
            var currentUser = requestHandler.GetCurrentUser();

            var likesResult = (await likeRepository.GetLikesAsync(currentUser))
                .Select(like => like.ToDTO());


            var pagination = requestHandler.GetInfoPagination();
            var result = new PagintaionResultDTO<LikeDTO>()
            {
                Page = pagination != null ? pagination.Page : 1,
                Records = pagination != null ? pagination.Records : likesResult.Count(),
                PagesCount = pagination != null ? (int)Math.Ceiling(Convert.ToDecimal(likesResult.Count() / pagination.Records)) : 1,
                Result = pagination != null ? likesResult.Skip((pagination.Page - 1) * pagination.Records).Take(pagination.Records) : likesResult
            };

            return ResponseResult<PagintaionResultDTO<LikeDTO>>.SetSuccessfully(result);
        }
        catch (Exception ex)
        {
            return ResponseResult<PagintaionResultDTO<LikeDTO>>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<LikeDTO>> LikedAsync(CreateLikeDTO like)
    {
        try
        {
            var pokemon = await pokemonRepository.GetAsync(like.PokemonId);
            if (pokemon == null)
                return ResponseResult<LikeDTO>.SetUnSuccessfully("Selected pokemon don't exist!");
            var currentUser = requestHandler.GetCurrentUser();
            if (pokemon.CreateBy != "Public" && pokemon.CreateBy != currentUser)
                return ResponseResult<LikeDTO>.SetUnSuccessfully("User can't like the pokemon that are not his own or public");

            var alreadyLike = await likeRepository.GetLikeByPokemonIdAndUser(pokemon.Id,currentUser);
            if (alreadyLike != null)
                return ResponseResult<LikeDTO>.SetUnSuccessfully("The pokemon already has a like assigned");

            Like newLike = new()
            {
                PokemonId = like.PokemonId,
                User = currentUser
            };

            await likeRepository.SaveLikedAsync(newLike);

            return ResponseResult<LikeDTO>.SetSuccessfully(newLike.ToDTO());
        }
        catch (Exception ex)
        {
            return ResponseResult<LikeDTO>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<LikeDTO>> RemoveLikeAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return ResponseResult<LikeDTO>.SetError("Id can't be empty");

            var existLike = await likeRepository.GetLikeAsync(id);
            if (existLike == null)
                return ResponseResult<LikeDTO>.SetUnSuccessfully("Like unexist");

            var currentUser = requestHandler.GetCurrentUser();
            if (existLike.User != currentUser)
                return ResponseResult<LikeDTO>.SetUnSuccessfully("User can't remove the Like that are not his own");

            await likeRepository.RemoveLikeAsync(existLike);

            return ResponseResult<LikeDTO>.SetSuccessfully();
        }
        catch (Exception ex)
        {
            return ResponseResult<LikeDTO>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<LikeDTO>> RemoveAllLikeAsync()
    {
        try
        {
            var currentUser = requestHandler.GetCurrentUser();
            
            await likeRepository.RemoveAllLikeAsync(currentUser);

            return ResponseResult<LikeDTO>.SetSuccessfully();
        }
        catch (Exception ex)
        {
            return ResponseResult<LikeDTO>.SetError(ex.Message);
        }
    }    
}
