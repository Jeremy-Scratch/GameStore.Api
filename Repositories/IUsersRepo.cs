using GameStore.Api.Entities;
namespace GameStore.Api.Repositories;
public interface IUsersRepo
{
    Task<int> Create(Users user);
    Task<Users?> CheckEmail(string email);
    Task<Users?> GetById(int id);
}