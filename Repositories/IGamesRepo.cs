using GameStore.Api.Entities;
namespace GameStore.Api.Repositories;
public interface IGamesRepo
{
    Task<int> AddGame(Games game);
    Task<Games?> GetGameById(int id);
    Task UpdateGame(Games game);
    public Task<IEnumerable<Games>> ListAllMovies();
    public Task DeleteGame(int id);
}