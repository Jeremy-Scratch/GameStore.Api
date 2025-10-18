namespace WebApp.Dtos;
public record class GameDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate
);