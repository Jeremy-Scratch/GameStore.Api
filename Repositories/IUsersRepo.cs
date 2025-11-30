using GameStore.Api.Entities;
namespace GameStore.Api.Repositories;
public interface IUsersRepo
{
    Task<Users?> GetById(int id);
    Task<Users?> CheckEmail(string email);
    Task<int> Create(Users user);
}