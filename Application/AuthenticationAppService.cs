using WebAPICrudPokemon.Application.Contracts;
using WebAPICrudPokemon.Domain.Contracts;
using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Helper;
using WebAPICrudPokemon.Settings;

namespace WebAPICrudPokemon.Application;

public class AuthenticationAppService : IAuthenticationAppService
{
    private readonly IAuthenticationRepository authenticationRepository;
    private readonly IJWTAuthenticationManager authenticationManager;

    public AuthenticationAppService(IAuthenticationRepository _authenticationRepository, IJWTAuthenticationManager _authenticationManager)
    {
        authenticationRepository = _authenticationRepository ?? throw new ArgumentNullException(nameof(IAuthenticationRepository));
        authenticationManager = _authenticationManager ?? throw new ArgumentNullException(nameof(IJWTAuthenticationManager));
    }

    public async Task<ResponseResult<UserDTO>> SignIn(UserDTO user)
    {
        try
        {
            var userValid = await authenticationRepository.SignIn(user.Email, user.Password);
            if (!userValid.IsSuccessful)
                return ResponseResult<UserDTO>.SetUnSuccessfully(userValid.Message);

            user.Token = authenticationManager.Authentication(user.Email);
            
            return ResponseResult<UserDTO>.SetSuccessfully(user);
        }
        catch (Exception ex)
        {
            return ResponseResult<UserDTO>.SetError(ex.Message);
        }
    }

    public async Task<ResponseResult<dynamic>> SignUp(UserDTO user)
    {
        try
        { 
            var result = await authenticationRepository.SignUp(user.Email, user.Password);
            return ResponseResult<dynamic>.SetSuccessfully();
        }
        catch (Exception ex)
        {
            return ResponseResult<dynamic>.SetError(ex.Message);
        }

    }
}
