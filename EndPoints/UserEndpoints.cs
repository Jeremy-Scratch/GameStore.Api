using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;
using GameStore.Api.Services;
using static BCrypt.Net.BCrypt;

namespace GameStore.Api.EndPoints;

public static class UserEndpoints
{
    const string route = "GetUser";
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users").WithParameterValidation();
        //Get User By Id
        group.MapGet("/{id}", async (int id, IUsersRepo usersRepo) =>
        {
            var user = await usersRepo.GetById(id);
            if (user is null)
            {
                return Results.NotFound();
            }

            var userDto = new UserDto
            (
                user!.Id,
                user.Name,
                user.Email,
                user.Role
            );
            return Results.Ok(userDto);
        }).WithName(route);
        //Create User
        group.MapPost("/", async (CreateUserDto newUser, IUsersRepo usersRepo) =>
        {
            var existUser = await usersRepo.CheckEmail(newUser.Email);
            if (existUser is not null)
            {
                return Results.Conflict("This Email is already in use");
            }

            var passwordHashed = HashPassword(newUser.Password);

            var user = new Users
            {
                Name = newUser.Name,
                Email = newUser.Email,
                PasswordHash = passwordHashed,
                Role = "Customer"
            };
            var newId = await usersRepo.Create(user);
            return Results.CreatedAtRoute(route, new { id = newId }, newUser);
        });
        //Login validation 
        group.MapPost("/login", async (LoginDto userLog, IUsersRepo usersRepo) =>
        {
            var user = await usersRepo.CheckEmail(userLog.Email);
            if (user is null)
            {
                return Results.Unauthorized();
            }
            var isValid = PasswordVerify.Check(userLog.Password, user.PasswordHash);
            if (!isValid)
            {
                return Results.Unauthorized();
            }
            var token = await TokenGenerator.GenerateToken();
            return Results.Ok(new { Token = token });
        });
        return group;
    }
}
