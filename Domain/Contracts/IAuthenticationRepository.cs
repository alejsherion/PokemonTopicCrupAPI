using WebAPICrudPokemon.Helper;
using WebAPICrudPokemon.Models;

namespace WebAPICrudPokemon.Domain.Contracts;

public interface IAuthenticationRepository
{
    Task<ResponseResult<User>> SignUp(string email, string password);

    Task<ResponseResult<User>> SignIn(string email, string password);
}
