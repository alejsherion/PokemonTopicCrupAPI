using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using WebAPICrudPokemon.Application;
using WebAPICrudPokemon.Application.Contracts;
using WebAPICrudPokemon.Domain;
using WebAPICrudPokemon.Domain.Contracts;
using WebAPICrudPokemon.Helper;
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

builder.Services.AddSingleton<RequestHandler>();
builder.Services.AddHttpContextAccessor();

// Healthy
builder.Services.AddHealthChecks()
    .AddMongoDb(
        mongoDBSettings.ConnectionString,
        name: mongoDBSettings.DataBaseName,
        timeout: TimeSpan.FromSeconds(3),
        tags: new[] { "ready" }
    );

// Add mongo client
builder.Services.AddSingleton<IMongoClient>(servicesProvider => new MongoClient(mongoDBSettings.ConnectionString));
// Add services to the container.
builder.Services.AddSingleton<ILikeAppService, LikeAppService>();
builder.Services.AddSingleton<IAuthenticationAppService, AuthenticationAppService>();
builder.Services.AddSingleton<IPokemonAppService, PokemonAppService>();
//builder.Services.AddSingleton<IPokemonRepository, InMemoryPokemonRepository>();
// Add Repositories
builder.Services.AddSingleton<ILikeRepository>(repo => new LikeRepository(repo.GetService<IMongoClient>(), mongoDBSettings.DataBaseName));
builder.Services.AddSingleton<IAuthenticationRepository>(repo => new AuthenticationRepository(repo.GetService<IMongoClient>(), mongoDBSettings.DataBaseName));
builder.Services.AddSingleton<IPokemonRepository>(repo => new PokemonRepository(repo.GetService<IMongoClient>(), mongoDBSettings.DataBaseName));
// Add Controllers
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

//Adding Healthy routes
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    nameof = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString()
                })
            }
        );

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.Run();
