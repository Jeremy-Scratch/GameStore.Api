using System.ComponentModel.DataAnnotations;
namespace WebApp.Dtos;
public record class UpdateGameDto(
    [Required][StringLength(50)]string Name,
    [Required]int GenreId,
    [Range(0, 100)]Decimal Price,
    DateTime ReleaseDate
);