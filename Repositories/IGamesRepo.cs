using GameStore.Api.Entities;
namespace GameStore.Api.Repositories;
public interface IGamesRepo
{
    Task<int> AddGame(Games game);
    Task<Games?> GetGameById(int id);
    void UpdateGame(Games game);
    public IEnumerable<Games> ListAllMovies();
    public void DeleteGame(int id);
    Task<int> CreateUser(Users user);
}