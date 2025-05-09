using MovieApp.Domain.Queries.Movies;
using MovieApp.Domain.Results.Movies;

namespace MovieApp.Services.Movies;

public interface IMoviesQueryService
{
    Task<List<MovieDto>> GetTopRatedMoviesAsync(CancellationToken token = default);
    Task<List<MovieDto>> GetLatestMoviesAsync(CancellationToken token = default);
    Task<List<MovieDto>> SearchMoviesAsync(SearchMoviesQuery query, CancellationToken token = default);
    Task<MovieDetailsDto> GetMovieDetailsAsync(int movieId, CancellationToken token = default);
}
