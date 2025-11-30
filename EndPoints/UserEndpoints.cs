using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;
using static BCrypt.Net.BCrypt;

namespace GameStore.Api.EndPoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints( this WebApplication app)
    {
        var group = app.MapGroup("users").WithParameterValidation();

        group.MapPost("/",  async (CreateUserDto newUser,IUsersRepo usersRepo) =>
        {
            var passwordHashed =  HashPassword(newUser.Password);
            var user = new Users
            {
                Name = newUser.Name,
                Email = newUser.Email,
                PasswordHash = passwordHashed,
                Role = "Customer"
            };
            var newId =await usersRepo.CreateUser(user);
            return Results.Ok(newUser);
        });

        return group;
    }
}
