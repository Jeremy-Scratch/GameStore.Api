using GameStore.Api.Entities;
namespace GameStore.Api.Repositories;
public interface IGamesRepo
{
    int AddGame(Games game);
    Games? GetGameById(int id);
    void UpdateGame(Games game);
    public IEnumerable<Games> ListAllMovies();
    public void DeleteGame(int id);
}