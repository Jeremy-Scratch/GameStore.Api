using System.Data;
using Npgsql;
using GameStore.Api.EndPoints;
using GameStore.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("DB_GAMESTORE");
//var connectionString = builder.Configuration.GetConnectionString("DB_GAMESTORE");     => if we want to use the variable in appsettings.json, can also be use for EnviromentVariable but we need to name with ConnectionString__ ("ConnectionStrings__DB_GAMES")
builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));
builder.Services.AddScoped<IGamesRepo, GamesRepo>();

var app = builder.Build();

app.MapGamesEndpoints();
app.Run();