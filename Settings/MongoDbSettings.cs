namespace WebAPICrudPokemon.Settings;

public class MongoDbSettings
{
    public string Host { get; set; }
    public string User { get; set; }
    public string DataBaseName { get; set; }
    public string Password { get; set; }

    public string ConnectionString { 
        get
        {
            return $"mongodb+srv://{User}:{Password}@{Host}/{DataBaseName}?retryWrites=true&w=majority";
        }
    }
}
