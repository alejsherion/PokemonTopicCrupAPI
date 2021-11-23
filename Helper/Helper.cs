using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPICrudPokemon.DTO;
using WebAPICrudPokemon.Models;

namespace WebAPICrudPokemon.Helper;

public class RequestHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;
    public RequestHandler(IHttpContextAccessor _httpContextAcessor)
        => httpContextAccessor = _httpContextAcessor ?? throw new ArgumentNullException(nameof(IHttpContextAccessor));

    internal string GetCurrentUser()
        => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
}


public static class Extensions
{
    public static bool IsNull(this object o)
        => o == null ?  throw new ArgumentNullException(nameof(o)) : true; 

    public static LikeDTO ToDTO(this Like l)
    {
        if (l == null) return null;

        return new()
        {
            Id = l.Id,
            PokemonId = l.PokemonId,
            User = l.User,
            CreateAt = l.CreateAt
        };
    }

    public static PokemonDTO ToDTO(this Pokemon p)
    {
        if (p == null) return null;

        return new ()
        {
            Id = p.Id,
            Name = p.Name,
            PokedexOrder = p.PokedexOrder,
            Region = p.Region,
            Type = p.Type,
            CreateAt = p.CreateAt
        };
    }

    public static Pokemon ToEntity(this CreatePokemonDTO dto)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            PokedexOrder = dto.PokedexOrder,
            Region = dto.Region,
            Type = dto.Type,
            CreateAt = DateTimeOffset.Now
        };
    }
    public static Pokemon ToEntity(this UpdatePokemonDTO dto)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            PokedexOrder = dto.PokedexOrder,
            Region = dto.Region,
            Type = dto.Type,
            CreateAt = DateTimeOffset.Now
        };
    }
}

public static class Crypt
{
    private const string SecureHashKey = "Secure_Key_Pokemon_For_Password";

    public static string Encrypt(string text)
    {
        try
        {
            // Getting the bytes of Input String.
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(text);

            MD5CryptoServiceProvider objMD5CryptoService = new ();
            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecureHashKey));
            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("An error ocurred in encrypt text", ex.Message);
        }
    }

    public static string Decrypt(string cipherText)
    {
        try
        {
            byte[] toEncryptArray = Convert.FromBase64String(cipherText);
            using MD5CryptoServiceProvider objMD5CryptoService = new();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecureHashKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider
            {
                //Assigning the Security key to the TripleDES Service Provider.
                Key = securityKeyArray,
                //Mode of the Crypto service is Electronic Code Book.
                Mode = CipherMode.ECB,
                //Padding Mode is PKCS7 if there is any extra byte is added.
                Padding = PaddingMode.PKCS7
            };

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            //Convert and return the decrypted data/byte into string format.
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("An error ocurred in decrypt text", ex.Message);
        }
    }
}

public class ResponseResult<T> 
{
    public bool IsSuccessful { get; private set; }
    public bool IsError { get; set; }
    public T Result { get; private set; }
    public string Message { get; private set; }

    protected ResponseResult(bool _isSuccessful, bool _isError, string _message)
    {
        IsSuccessful = _isSuccessful;
        IsError = _isError;
        Message = _message;
    }
    protected ResponseResult(bool _isSuccessful, string _message, T _result) 
    {
        IsSuccessful = _isSuccessful;
        Result = _result;
        Message = _message;
    }

    public static ResponseResult<T> SetSuccessfully() => new(true, false, "");
    public static ResponseResult<T> SetSuccessfully(T result) => new(true, "", result);
    public static ResponseResult<T> SetUnSuccessfully(string message) => new(false, false, message);
    public static ResponseResult<T> SetError(string message) => new(false, true, message);
}