using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Text;
using WebAPICrudPokemon.Application;
using WebAPICrudPokemon.Domain;
using WebAPICrudPokemon.Settings;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

var mongoDBSettings = builder.Configuration
    .GetSection(nameof(MongoDbSettings))
    .Get<MongoDbSettings>();

const string AuthKey = "WRDSDE-dqwdsoWQ-245-2132-dqdqw-w";

// Add Authentication
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearer =>
{
    bearer.RequireHttpsMetadata = false;
    bearer.SaveToken = true;
    bearer.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(AuthKey));

// Add mongo client
builder.Services.AddSingleton<IMongoClient>(servicesProvider => new MongoClient(mongoDBSettings.ConnectionString));

// Add services to the container.
builder.Services.AddSingleton<IAuthenticationAppService, AuthenticationAppService>();
builder.Services.AddSingleton<IPokemonAppService, PokemonAppService>();
//builder.Services.AddSingleton<IPokemonRepository, InMemoryPokemonRepository>();
builder.Services.AddSingleton<IAuthenticationRepository>(repo => new AuthenticationRepository(repo.GetService<IMongoClient>(), mongoDBSettings.DataBaseName));
builder.Services.AddSingleton<IPokemonRepository>(repo => new PokemonRepository(repo.GetService<IMongoClient>(), mongoDBSettings.DataBaseName));

builder.Services.AddControllers(controllers => controllers.SuppressAsyncSuffixInActionNames = false);
            
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
