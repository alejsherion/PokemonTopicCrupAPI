using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.Application.Contracts;

public interface IAuthenticationAppService
{
    Task<ResponseResult<dynamic>> SignUp(UserDTO user);

    Task<ResponseResult<UserDTO>> SignIn(UserDTO user);
}