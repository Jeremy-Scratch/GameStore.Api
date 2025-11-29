using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;
using System.Linq;

namespace GameStore.Api.EndPoints;
public static class GameEndpoints
{
    const string GetGameEndPointName = "GetGame";
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        //GET /games
        group.MapGet("/", (IGamesRepo gamesRepo) => 
        {
            var games = gamesRepo.ListAllMovies().ToList();
            var clientGameList = games.Select(l => new GameDto(l.Id, l.Name, l.GenreId!.Name!, l.Price, l.ReleaseDate)).ToList();
            return Results.Ok(clientGameList);
        });

        //GET /games/1 BY ID
        group.MapGet("/{id}", (int id,IGamesRepo gamesRepo) =>
        {
            var game = gamesRepo.GetGameById(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            var responseDto = new GameDto
            (
                game.Id,
                game.Name,
                game.GenreId!.Name!,
                game.Price,
                game.ReleaseDate
            );
            return Results.Ok(responseDto);
        }).WithName(GetGameEndPointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame, IGamesRepo gamesRepo) =>
        {
            var game = new Games
            {
                Name = newGame.Name,
                GenreId = new Genres{Id = newGame.GenreId},
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            var newId = gamesRepo.AddGame(game);
            return Results.CreatedAtRoute(GetGameEndPointName, new { id =newId },newGame);
        });

        // PUT/games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedDto, IGamesRepo gamesRepo) =>
        {
            var updatedGame = new Games
            {
                Id = id,
                Name = updatedDto.Name,
                GenreId = new Genres{Id = updatedDto.GenreId},
                Price = updatedDto.Price,
                ReleaseDate = updatedDto.ReleaseDate
            };
            gamesRepo.UpdateGame(updatedGame);
            return Results.NoContent();
        });

        //DELETE/ games
        group.MapDelete("/{id}", (int id, IGamesRepo gamesRepo) =>
        {
           gamesRepo.DeleteGame(id);

            return Results.NoContent();
        });
        return group;
    }
}