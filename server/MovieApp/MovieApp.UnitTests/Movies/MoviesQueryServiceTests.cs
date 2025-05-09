using MovieApp.Domain.Queries.Movies;
using MovieApp.Domain.Results.Movies;
using MovieApp.Domain.Results;
using MovieApp.Infrastructure.Caching;
using Moq;
using MovieApp.Domain.Models;
using TMDbService.Services;
using MovieApp.Services.Movies;

namespace MovieApp.UnitTests.Movies;

public class MoviesQueryServiceTests
{
    private readonly Mock<ITMDBQueryService> _tmdbMock = new();
    private readonly Mock<IAppCache> _cacheMock = new();
    private readonly MoviesQueryService _service;

    public MoviesQueryServiceTests()
    {
        _service = new MoviesQueryService(_tmdbMock.Object, _cacheMock.Object, TestDbContextFactory.CreateInMemoryDbContext());
    }

    [Fact]
    public async Task TestGetTopRatedMoviesAsync_CachedMovies_ShouldReturnFromCache()
    {
        List<MovieDto> expected =
        [
            new()
            {
                Id = 1,
                Title = "Cached Movie"
            }
        ];
        _cacheMock.Setup(c => c.Get<List<MovieDto>>(CacheConstants.TopRatedMovies))
            .Returns(expected);

        var result = await _service.GetTopRatedMoviesAsync();

        Assert.Equal(expected, result);
        _tmdbMock.Verify(t => t.GetTopRatedMoviesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task TestGetTopRatedMoviesAsync_NoCachedResults_ShouldReturnFromTMDbService()
    {
        const string movieTitle = "Fetched Movie";

        _cacheMock.Setup(c => c.Get<List<MovieDto>>(CacheConstants.TopRatedMovies))
            .Returns((List<MovieDto>?)null);

        var tmdbResult = CallResult<List<MovieDto>>.Success([new() { Id = 2, Title = movieTitle }]);
        _tmdbMock.Setup(t => t.GetTopRatedMoviesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tmdbResult);

        var result = await _service.GetTopRatedMoviesAsync(CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(movieTitle, result[0].Title);
        _cacheMock.Verify(c => c.Set(CacheConstants.TopRatedMovies, It.IsAny<List<MovieDto>>()), Times.Once);
    }

    [Fact]
    public async Task TestSearchMoviesAsync_WhenTMDbServiceFails_ShouldReturnEmptyList()
    {
        var query = new SearchMoviesQuery { Query = "Test" };

        _cacheMock.Setup(c => c.Get<List<MovieDto>>(It.IsAny<string>())).Returns((List<MovieDto>?)null);
        _tmdbMock.Setup(t => t.SearchMoviesAsync("Test", null, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(CallResult<List<MovieDto>>.Failure("error"));

        var result = await _service.SearchMoviesAsync(query);

        Assert.Empty(result);
    }

    [Fact]
    public async Task TestGetMovieDetailsAsync_WhenTMDbServiceAvailable_ShouldReturnResult()
    {
        const int movieId = 99;

        var movieDetails = new MovieDetailsDto { Id = movieId, Title = "Details" };

        _tmdbMock.Setup(t => t.GetMovieDetailsAsync(movieId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(CallResult<MovieDetailsDto>.Success(movieDetails));

        var result = await _service.GetMovieDetailsAsync(99);

        Assert.Equal(movieId, result.Id);
    }

    [Fact]
    public async Task TestGetMovieDetailsAsync_WhenTMDbServiceFails_ShouldThrowError()
    {
        const string errorMessage = "Not Found";

        _tmdbMock.Setup(t => t.GetMovieDetailsAsync(99, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(CallResult<MovieDetailsDto>.Failure(errorMessage));

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetMovieDetailsAsync(99));
        Assert.Equal(errorMessage, ex.Message);
    }

    [Fact]
    public async Task TestGetMovieDetailsAsync_WithCommentsInDatabase_ShouldReturnWithComments()
    {
        const int movieId = 101;

        var dbContext = TestDbContextFactory.CreateInMemoryDbContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser"
        };

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            MovieId = movieId,
            Content = "TestContent!",
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        dbContext.Comments.Add(comment);
        await dbContext.SaveChangesAsync();

        var movieDetails = new MovieDetailsDto { Id = movieId, Title = "Test Movie" };

        var tmdbMock = new Mock<ITMDBQueryService>();
        var cacheMock = new Mock<IAppCache>();

        tmdbMock.Setup(t => t.GetMovieDetailsAsync(movieId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CallResult<MovieDetailsDto>.Success(movieDetails));

        var service = new MoviesQueryService(tmdbMock.Object, cacheMock.Object, dbContext);

        var result = await service.GetMovieDetailsAsync(movieId);

        Assert.Equal(movieId, result.Id);
        Assert.Single(result.Comments);
        Assert.Equal(comment.Content, result.Comments[0].Content);
        Assert.Equal(user.Username, result.Comments[0].User.Username);
    }
}