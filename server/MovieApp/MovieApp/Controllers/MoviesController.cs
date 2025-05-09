using Microsoft.AspNetCore.Mvc;
using MovieApp.Domain.Queries.Movies;
using MovieApp.Services.Movies;

namespace MovieApp.API.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly IMoviesQueryService _moviesQueryService;

    public MoviesController(IMoviesQueryService moviesQueryService)
    {
        _moviesQueryService = moviesQueryService;
    }

    [HttpGet("top-rated")]
    public async Task<IActionResult> GetTopRated(CancellationToken token = default)
    {
        var result = await _moviesQueryService.GetTopRatedMoviesAsync(token);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchMoviesQuery query, CancellationToken token = default)
    {
        var result = await _moviesQueryService.SearchMoviesAsync(query, token);

        return Ok(result);
    }

    [HttpGet("{movieId}")]
    public async Task<IActionResult> GetDetails([FromRoute] int movieId, CancellationToken token)
    {
        var result = await _moviesQueryService.GetMovieDetailsAsync(movieId, token);

        return Ok(result);
    }
}