using Dapper;
using GameStore.Api.Entities;
using System.Data;

namespace GameStore.Api.Repositories;

public class GamesRepo : IGamesRepo 
{
    private readonly IDbConnection _connection;
    public GamesRepo (IDbConnection connection)
    {
        _connection = connection;
        _connection.Open();
    }
    public async Task<int> Add(Games game)
    {
        var sql = "INSERT INTO games(name,\"genreId\",price,\"releaseDate\") VALUES (@Name, @GenreId, @Price, @ReleaseDate) RETURNING id";
        var newId = await _connection.ExecuteScalarAsync<int>(sql, new
        {
            game.Name,
            GenreId = game.GenreId.Id,
            game.Price,
            game.ReleaseDate
        });
        return newId;
    }
    public async Task<Games?> GetById(int id)
    {
        var sql = "SELECT g.id , g.name, g.price, g.\"releaseDate\", ge.id , ge.name FROM genres AS ge JOIN games AS g ON ge.id = g.\"genreId\" WHERE g.id = @Id";
        var game = await _connection.QueryAsync<Games, Genres, Games>(sql,
        (Game, Genre) =>
        {
            Game.GenreId = Genre;
            return Game;
        },
        new { Id = id }, splitOn: "id")!;

        return game.FirstOrDefault();
    }
    public async Task Update(Games game)
    {
        var sql = "UPDATE games SET name = @Name, \"genreId\" = @GenreId, price = @Price, \"releaseDate\" = @ReleaseDate WHERE id = @Id";
        await _connection.ExecuteAsync(sql, new
        {
            game.Id,
            game.Name,
            GenreId = game.GenreId.Id,
            game.Price,
            game.ReleaseDate
        });
    }
    public async Task<IEnumerable<Games>> ListAll()
    {
        var sql = "SELECT g.id , g.name, g.price, g.\"releaseDate\", ge.id , ge.name FROM genres AS ge JOIN games AS g ON ge.id = g.\"genreId\"";
        var games = await _connection.QueryAsync<Games, Genres, Games>(sql,
        (Game, Genre) =>
        {
            Game.GenreId = Genre;
            return Game;
        }, splitOn: "id");
        return games;
    }
    public async Task Delete(int id)
    {
       await _connection.ExecuteAsync("DELETE FROM games Where id = @Id", new { Id = id });
    }
}