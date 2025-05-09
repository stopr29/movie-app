namespace MovieApp.Domain.Queries.Movies;

public class SearchMoviesQuery
{
    public string Query { get; set; } = string.Empty;
    public int? GenreId { get; set; }
}