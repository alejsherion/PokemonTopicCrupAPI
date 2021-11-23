using MongoDB.Bson;
using MongoDB.Driver;
using WebAPICrudPokemon.Models;
using WebAPICrudPokemon.Settings;

namespace WebAPICrudPokemon.Domain
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly List<Pokemon> InMemoryPokemons = new()
        {
            new Pokemon() { Id = Guid.NewGuid(), Name = "Charmander", PokedexOrder = 4, Region = "Kanto", Type = "Fire", CreateAt = DateTime.Now, CreateBy="Public" },
            new Pokemon() { Id = Guid.NewGuid(), Name = "Bagon", PokedexOrder = 371, Region = "Hoen", Type = "Dragon", CreateAt = DateTime.Now, CreateBy="Public" },
            new Pokemon() { Id = Guid.NewGuid(), Name = "Totodile", PokedexOrder = 158, Region = "Johto", Type = "Water", CreateAt = DateTime.Now, CreateBy = "Public" },
            new Pokemon() { Id = Guid.NewGuid(), Name = "Ditto", PokedexOrder = 132, Region = "Kanto", Type = "Normal", CreateAt = DateTime.Now, CreateBy = "Public" },
            new Pokemon() { Id = Guid.NewGuid(), Name = "Zoroark", PokedexOrder = 571, Region = "Kalos", Type = "Sinister", CreateAt = DateTime.Now, CreateBy = "Public" },
            new Pokemon() { Id = Guid.NewGuid(), Name = "Zeraora", PokedexOrder = 807, Region = "Teselia", Type = "Electric", CreateAt = DateTime.Now, CreateBy = "Public" },
        };

        private readonly IMongoCollection<Pokemon> PokemonCollection;
        private readonly FilterDefinitionBuilder<Pokemon> FilterBuilder = Builders<Pokemon>.Filter;

        public PokemonRepository(IMongoClient client, string DatabaseName)
        {
            IMongoDatabase database = client.GetDatabase(DatabaseName);
            PokemonCollection = database.GetCollection<Pokemon>(nameof(Pokemon));

            InitialLoadPokemonsAsync();
        }

        private void InitialLoadPokemonsAsync()
        {
            var documents = PokemonCollection.Find(new BsonDocument()).ToList();
            if (documents.Count == 0)
                PokemonCollection.InsertMany(InMemoryPokemons);
        }

        public async Task<IEnumerable<Pokemon>> GetPokemonsAsync()
            => (await PokemonCollection.FindAsync(new BsonDocument())).ToList();

        public async Task<Pokemon> GetAsync(Guid id)
        {
            var filter = FilterBuilder.Eq(pokemon => pokemon.Id, id);
            return (await PokemonCollection.FindAsync(filter)).SingleOrDefault();
        }

        public async Task<Pokemon> GetByNameAsync(string name)
        {
            var filter = FilterBuilder.Eq(pokemon => pokemon.Name, name);
            return (await PokemonCollection.FindAsync(filter)).SingleOrDefault();
        }

        public async Task AddAsync(Pokemon pokemon) 
            => await PokemonCollection.InsertOneAsync(pokemon);

        public async Task UpdateAsync(Pokemon pokemon)
        {
            var filter = FilterBuilder.Eq(pokemon => pokemon.Id, pokemon.Id);
            await PokemonCollection.ReplaceOneAsync(filter, pokemon);
        }
        
        public async Task RemoveAsync(Pokemon pokemon)
        {
            var filter = FilterBuilder.Eq(pokemon => pokemon.Id, pokemon.Id);
            await PokemonCollection.DeleteOneAsync(filter);
        }
    }
}
