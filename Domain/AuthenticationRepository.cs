using MongoDB.Driver;
using WebAPICrudPokemon.Helper;
using WebAPICrudPokemon.Models;

namespace WebAPICrudPokemon.Domain;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly IMongoCollection<User> UserCollection;
    private readonly FilterDefinitionBuilder<User> FilterBuilder = Builders<User>.Filter;

    public AuthenticationRepository(IMongoClient client, string DatabaseName)
    {
        IMongoDatabase database = client.GetDatabase(DatabaseName);
        UserCollection = database.GetCollection<User>(nameof(User));
    }

    public async Task<ResponseResult<User>> SignIn(string email, string password)
    {
        var filter = FilterBuilder.Eq(user => user.Email, email);
        var registeredUser = (await UserCollection.FindAsync(filter)).FirstOrDefault();
        if (registeredUser is null)
            return ResponseResult<User>.SetUnSuccess("Unregistered user");
        if (registeredUser.Password != Crypt.Encrypt(password))
            return ResponseResult<User>.SetUnSuccess("Invalid password");

        return ResponseResult<User>.SetSuccess(registeredUser);
    }

    public async Task<ResponseResult<User>> SignUp(string email, string password)
    {
        var filter = FilterBuilder.Eq(user => user.Email, email);
        var registeredUser = (await UserCollection.FindAsync(filter)).FirstOrDefault();
        if (registeredUser is not null)
            return ResponseResult<User>.SetUnSuccess("User Already registered!");
        
        var newUser = new User() { Email = email, Password = Crypt.Encrypt(password) };
        await UserCollection.InsertOneAsync(newUser);
        return ResponseResult<User>.SetSuccess(newUser);
    }
}
