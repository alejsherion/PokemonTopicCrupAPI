using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Helper;

namespace WebAPICrudPokemon.Application;

public interface IAuthenticationAppService
{
    Task<ResponseResult<dynamic>> SignUp(UserDTO user);

    Task<ResponseResult<UserDTO>> SignIn(UserDTO user);
}