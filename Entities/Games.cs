namespace WebApp.Entities;
public class Games
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Genres GenreId { get; set; }
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
}