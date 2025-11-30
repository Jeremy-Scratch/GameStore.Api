using GameStore.Api.Entities;
namespace GameStore.Api.Repositories;
public interface IUsersRepo
{
    Task<int> CreateUser(Users user);
}
