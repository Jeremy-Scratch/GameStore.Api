namespace GameStore.Api.Dtos;
public record class UserDto
(
    int Id,
    string Name,
    string Email,
    string Role
);