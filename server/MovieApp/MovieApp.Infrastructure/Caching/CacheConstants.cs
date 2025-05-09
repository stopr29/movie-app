namespace MovieApp.Infrastructure.Caching;

public static class CacheConstants
{
    public const string TopRatedMovies = "movies:top-rated";
    public const string LatestMovies = "movies:latest";

    public static string SearchMovies(string query, int? genreId) =>
        $"movies:search:{query}:{genreId}";
}