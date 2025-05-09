using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MovieApp.Domain.Results;
using MovieApp.Domain.Results.Movies;
using TMDbService.Responses;
using System.Net.Http.Json;

namespace TMDbService.Services;

public class TMDBQueryService : ITMDBQueryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TMDBQueryService> _logger;

    public TMDBQueryService(HttpClient httpClient,
        ILogger<TMDBQueryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CallResult<List<MovieDto>>> GetTopRatedMoviesAsync(CancellationToken token = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<TMDbResponse<MovieDto>>(
                "movie/top_rated?language=en-US", token);

            return CallResult<List<MovieDto>>.Success(response?.Results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch top-rated movies");
            return CallResult<List<MovieDto>>.Failure("Failed to fetch top rated movies: " + ex.Message);
        }
    }

    public async Task<CallResult<List<MovieDto>>> SearchMoviesAsync(string query, int? genreId = null, CancellationToken token = default)
    {
        try
        {
            var parameters = new Dictionary<string, string>
            {
                { "language", "en-US" },
                { "query", query }
            };

            if (genreId.HasValue)
            {
                parameters.Add("with_genres", genreId.Value.ToString());
            }

            var url = QueryHelpers.AddQueryString("search/movie", parameters!);

            var response = await _httpClient.GetFromJsonAsync<TMDbResponse<MovieDto>>(url, token);

            return CallResult<List<MovieDto>>.Success(response?.Results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search for movies. Query: {Query}, GenreId: {GenreId}", query, genreId);
            return CallResult<List<MovieDto>>.Failure("Failed to search movies: " + ex.Message);
        }
    }
    
    public async Task<CallResult<MovieDetailsDto>> GetMovieDetailsAsync(int movieId, CancellationToken token = default)
    {
        try
        {
            var url = $"movie/{movieId}?language=en-US&append_to_response=images,credits";

            var response = await _httpClient.GetFromJsonAsync<MovieDetailsDto>(url, token);

            if (response == null)
            {
                _logger.LogWarning("Movie details not found. MovieId: {MovieId}", movieId);
                return CallResult<MovieDetailsDto>.Failure("Movie not found");
            }

            return CallResult<MovieDetailsDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch movie details. MovieId: {MovieId}", movieId);
            return CallResult<MovieDetailsDto>.Failure("Failed to fetch movie details: " + ex.Message);
        }
    }
}