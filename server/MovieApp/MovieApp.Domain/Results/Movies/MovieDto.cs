namespace MovieApp.Domain.Results.Movies;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string PosterPath { get; set; } = string.Empty;
    public double VoteAverage { get; set; }
    public string ReleaseDate { get; set; } = string.Empty;
}