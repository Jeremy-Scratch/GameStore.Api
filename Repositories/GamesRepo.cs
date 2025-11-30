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
    public async Task<int> AddGame(Games game)
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
    public Games? GetGameById(int id)
    {
        var sql = "SELECT g.id , g.name, g.price, g.\"releaseDate\", ge.id , ge.name FROM genres AS ge JOIN games AS g ON ge.id = g.\"genreId\" WHERE g.id = @Id";
        return _connection.Query<Games, Genres, Games>(sql,
        (Games, Genres) =>
        {
            Games.GenreId = Genres;
            return Games;
        }, new { Id = id }, splitOn: "id")!.FirstOrDefault();
    }
    public void UpdateGame(Games game)
    {
        var sql = "UPDATE games SET name = @Name, \"genreId\" = @GenreId, price = @Price, \"releaseDate\" = @ReleaseDate WHERE id = @Id";
        _connection.Execute(sql, new
        {
            game.Id,
            game.Name,
            GenreId = game.GenreId.Id,
            game.Price,
            game.ReleaseDate
        });
    }
    public IEnumerable<Games> ListAllMovies()
    {
        var sql = "SELECT g.id , g.name, g.price, g.\"releaseDate\", ge.id , ge.name FROM genres AS ge JOIN games AS g ON ge.id = g.\"genreId\"";
        return _connection.Query<Games, Genres, Games>(sql,
        (Games, Genres) =>
        {
            Games.GenreId = Genres;
            return Games;
        }, splitOn: "id");
    }
    public void DeleteGame(int id)
    {
        _connection.Execute("DELETE FROM games Where id = @Id", new { Id = id });
    }

    public async Task<int> CreateUser(Users user)
    {
        var sql = "INSERT INTO users(name,email,\"passwordHash\",role) VALUES (@Name, @Email, @PasswordHash, @Role) RETURNING id";
        var newId = await _connection.ExecuteScalarAsync<int>(sql, new
        {
            user.Name,
            user.Email,
            user.PasswordHash,
            user.Role
        });
        return newId;
    }
}