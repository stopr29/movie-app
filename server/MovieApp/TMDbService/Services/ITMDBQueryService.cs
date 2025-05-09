using MovieApp.Domain.Results;
using MovieApp.Domain.Results.Movies;

namespace TMDbService.Services;

public interface ITMDBQueryService
{
    Task<CallResult<List<MovieDto>>> GetTopRatedMoviesAsync(CancellationToken token = default);
    Task<CallResult<List<MovieDto>>> SearchMoviesAsync(string query, int? genreId = null, CancellationToken token = default);
    Task<CallResult<MovieDetailsDto>> GetMovieDetailsAsync(int movieId, CancellationToken token = default);
}