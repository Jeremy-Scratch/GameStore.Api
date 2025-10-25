using Npgsql;
using Dapper;
using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class GamesRepo
{
    private readonly string _ConnectionString = Environment.GetEnvironmentVariable("DB_GAMESTORE", EnvironmentVariableTarget.User)!;
    public int AddGame(Games game)
    {
        using var connection = new NpgsqlConnection(_ConnectionString);
        connection.Open();
        var sql = "INSERT INTO games(name,\"genreId\",price,\"releaseDate\") VALUES (@Name, @GenreId, @Price, @ReleaseDate) RETURNING id";
        var newId = connection.ExecuteScalar<int>(sql, new
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
        using var connection = new NpgsqlConnection(_ConnectionString);
        var sql = "SELECT g.id , g.name, g.price, g.\"releaseDate\", ge.id , ge.name FROM genres AS ge JOIN games AS g ON ge.id = g.\"genreId\" WHERE g.id = @Id";
        return connection.Query<Games, Genres, Games>(sql,
        (Games, Genres) =>
        {
            Games.GenreId = Genres;
            return Games;
        }, new { Id = id }, splitOn: "id")!.FirstOrDefault();
    }
    public void UpdateGame(Games game)
    {
        using var connection = new NpgsqlConnection(_ConnectionString);
        var sql = "UPDATE games SET name = @Name, \"genreId\" = @GenreId, price = @Price, \"releaseDate\" = @ReleaseDate WHERE id = @Id";
        connection.Execute(sql, new
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
        using var connection = new NpgsqlConnection(_ConnectionString);
        var sql = "SELECT g.id , g.name, g.price, g.\"releaseDate\", ge.id , ge.name FROM genres AS ge JOIN games AS g ON ge.id = g.\"genreId\"";
        return connection.Query<Games, Genres, Games>(sql,
        (Games, Genres) =>
        {
            Games.GenreId = Genres;
            return Games;
        }, splitOn: "id");
    }
    public void DeleteGame(int id)
    {
        using var connection = new NpgsqlConnection(_ConnectionString);
        connection.Execute("DELETE FROM games Where id = @Id", new { Id = id });
    }
}