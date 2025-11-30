using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using static BCrypt.Net.BCrypt;

namespace GameStore.Api.EndPoints;

public class UserEndpoints
{
    public RouteGroupBuilder MapUserEndpoints(WebApplication app)
    {
        var group = app.MapGroup("users").WithParameterValidation();

        group.MapPost("/", (CreateUserDto newUser) =>
        {

            var passwordHashed = HashPassword(newUser.Password);
            var user = new Users
            {
                Name = newUser.Name,
                Email = newUser.Email,
                PasswordHash = passwordHashed,
                Role = "Customer"
            };
            return Results.Ok();
        });

        return group;
    }
    

    

}
