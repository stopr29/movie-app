using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Queries.Movies;
using MovieApp.Domain.Results.Comments;
using MovieApp.Domain.Results.Movies;
using MovieApp.Domain.Results.Users;
using MovieApp.Infrastructure.Caching;
using MovieApp.Infrastructure.Persistence;
using TMDbService.Services;

namespace MovieApp.Services.Movies;

public class MoviesQueryService : IMoviesQueryService
{
    private readonly ITMDBQueryService _tmdbQueryService;
    private readonly MovieAppDbContext _context;
    private readonly IAppCache _cache;

    public MoviesQueryService(ITMDBQueryService tmdb,
        IAppCache cache,
        MovieAppDbContext context)
    {
        _tmdbQueryService = tmdb;
        _cache = cache;
        _context = context;
    }

    public async Task<List<MovieDto>> GetTopRatedMoviesAsync(CancellationToken token = default)
    {
        var cached = _cache.Get<List<MovieDto>>(CacheConstants.TopRatedMovies);
        if (cached is not null)
        {
            return cached;
        }

        var result = await _tmdbQueryService.GetTopRatedMoviesAsync(token);
        var data = result.IsSuccess && result.Data != null ? result.Data : [];

        _cache.Set(CacheConstants.TopRatedMovies, data);
        return data;
    }

    public async Task<List<MovieDto>> GetLatestMoviesAsync(CancellationToken token = default)
    {
        var cached = _cache.Get<List<MovieDto>>(CacheConstants.LatestMovies);
        if (cached is not null)
        {
            return cached;
        }

        var result = await _tmdbQueryService.SearchMoviesAsync("", null, token);

        var data = result.IsSuccess && result.Data != null
            ? result.Data.ToList()
            : [];

        _cache.Set(CacheConstants.LatestMovies, data);
        return data;
    }

    public async Task<List<MovieDto>> SearchMoviesAsync(SearchMoviesQuery query, CancellationToken token = default)
    {
        var cacheKey = CacheConstants.SearchMovies(query.Query, query.GenreId);

        var cached = _cache.Get<List<MovieDto>>(cacheKey);
        if (cached is not null)
        {
            return cached;
        }

        var result = await _tmdbQueryService.SearchMoviesAsync(query.Query, query.GenreId, token);
        var data = result.IsSuccess && result.Data != null ? result.Data : [];

        _cache.Set(cacheKey, data);
        return data;
    }

    public async Task<MovieDetailsDto> GetMovieDetailsAsync(int movieId, CancellationToken token = default)
    {
        var result = await _tmdbQueryService.GetMovieDetailsAsync(movieId, token);

        if (!result.IsSuccess)
        {
            throw new Exception(result.ErrorMessage);
        }

        var movieDetails = result.Data!;

        var comments = await _context.Comments
            .Where(c => c.MovieId == movieId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                User = new UserDto
                {
                    Id = c.UserId,
                    Username = c.User.Username,
                },
                Content = c.Content,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync(token);

        movieDetails.Comments = comments;

        return movieDetails;
    }
}
