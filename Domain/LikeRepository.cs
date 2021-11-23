using MongoDB.Driver;
using WebAPICrudPokemon.Domain.Contracts;
using WebAPICrudPokemon.Models;

namespace WebAPICrudPokemon.Domain;

public class LikeRepository : ILikeRepository
{
    private readonly IMongoCollection<Like> LikeColletion;
    private readonly FilterDefinitionBuilder<Like> FilterBuilder = Builders<Like>.Filter;

    public LikeRepository(IMongoClient client, string databaseName)
    {
        IMongoDatabase database = client.GetDatabase(databaseName);
        LikeColletion = database.GetCollection<Like>(nameof(Like));
    }

    public async Task<IEnumerable<Like>> GetLikesAsync(string user)
    {
        var filter = FilterBuilder.Eq(like => like.User, user);
        return (await LikeColletion.FindAsync(filter)).ToList();
    }
    
    public async Task<Like> GetLikeAsync(Guid id)
    {
        var filter = FilterBuilder.Eq(like => like.Id, id);
        return (await LikeColletion.FindAsync(filter)).FirstOrDefault();
    }

    public async Task<Like> GetLikeByPokemonIdAndUser(Guid pokemonId, string user)
    {
        var filter = FilterBuilder.Where(like => like.PokemonId == pokemonId && like.User == user);
        return (await LikeColletion.FindAsync(filter)).FirstOrDefault();
    }

    public async Task SaveLikedAsync(Like like)
        => await LikeColletion.InsertOneAsync(like);


    public async Task RemoveLikeAsync(Like like)
    {
        var filter = FilterBuilder.Eq(like => like.Id, like.Id);
        await LikeColletion.DeleteOneAsync(filter);
    }

    public async Task RemoveAllLikeAsync(string user)
    {
        var filter = FilterBuilder.Eq(like => like.User, user);
        await LikeColletion.DeleteManyAsync(filter);
    }

}