using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;


namespace GameStore.Api.EndPoints;
public static class GameEndpoints
{
    private static GamesRepo gr = new GamesRepo();
    const string GetGameEndPointName = "GetGame";
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        //GET /games
        group.MapGet("/", () =>
        {
            var games = (List<Games>)gr.ListAllMovies();
            var clientGameList = games.Select(l => new GameDto(l.Id, l.Name, l.GenreId!.Name!, l.Price, l.ReleaseDate)).ToList();
            return Results.Ok(clientGameList);
        });

        //GET /games/1 BY ID
        group.MapGet("/{id}", (int id) =>
        {
            var game = gr.GetGameById(id);
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
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            var game = new Games
            {
                Name = newGame.Name,
                GenreId = new Genres{Id = newGame.GenreId},
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            var newId = gr.AddGame(game);
            return Results.CreatedAtRoute(GetGameEndPointName, new { id =newId },newGame);
        });

        // PUT/games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedDto) =>
        {
            var updatedGame = new Games
            {
                Id = id,
                Name = updatedDto.Name,
                GenreId = new Genres{Id = updatedDto.GenreId},
                Price = updatedDto.Price,
                ReleaseDate = updatedDto.ReleaseDate
            };
            gr.UpdateGame(updatedGame);
            return Results.NoContent();
        });

        //DELETE/ games
        group.MapDelete("/{id}", (int id) =>
        {
           gr.DeleteGame(id);

            return Results.NoContent();
        });
        return group;
    }
}