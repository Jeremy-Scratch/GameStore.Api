using GameStore.Api.Entities;
namespace GameStore.Api.Repositories;
public interface IGamesRepo
{
    Task<int> Add(Games game);
    Task<Games?> GetById(int id);
    Task Update(Games game);
    public Task<IEnumerable<Games>> ListAll();
    public Task Delete(int id);
}