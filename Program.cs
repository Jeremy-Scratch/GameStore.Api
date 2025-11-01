using System.Data;
using Npgsql;
using GameStore.Api.EndPoints;
using GameStore.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("DB_GAMESTORE");
builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));
builder.Services.AddScoped<IGamesRepo, GamesRepo>();

var app = builder.Build();

app.MapGamesEndpoints();
app.Run();