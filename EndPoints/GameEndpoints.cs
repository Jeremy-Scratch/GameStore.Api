using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.EndPoints;
public static class GameEndpoints
{
    const string route = "GetGame";
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        //GET /games
        group.MapGet("/", async (IGamesRepo gamesRepo) => 
        {
            var games = await gamesRepo.ListAll();
            var clientGameList = games.Select(l => new GameDto(l.Id, l.Name, l.GenreId!.Name!, l.Price, l.ReleaseDate)).ToList();
            return Results.Ok(clientGameList);
        });
        //GET /games/1 BY ID
        group.MapGet("/{id}", async (int id,IGamesRepo gamesRepo) =>
        {
            var game = await gamesRepo.GetById(id);
            
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
        }).WithName(route);
        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, IGamesRepo gamesRepo) =>
        {
            var game = new Games
            {
                Name = newGame.Name,
                GenreId = new Genres{Id = newGame.GenreId},
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            var newId = await gamesRepo.Add(game);
            return Results.CreatedAtRoute(route, new { id =newId },newGame);
        });
        // PUT/games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedDto, IGamesRepo gamesRepo) =>
        {
            var updatedGame = new Games
            {
                Id = id,
                Name = updatedDto.Name,
                GenreId = new Genres{Id = updatedDto.GenreId},
                Price = updatedDto.Price,
                ReleaseDate = updatedDto.ReleaseDate
            };
            await gamesRepo.Update(updatedGame);
            return Results.NoContent();
        });
        //DELETE/ games
        group.MapDelete("/{id}", async (int id, IGamesRepo gamesRepo) =>
        {
           await gamesRepo.Delete(id);

            return Results.NoContent();
        });
        return group;
    }
}