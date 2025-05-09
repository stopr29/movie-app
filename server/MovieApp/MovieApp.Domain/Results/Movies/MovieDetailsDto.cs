using MovieApp.Domain.Results.Comments;

namespace MovieApp.Domain.Results.Movies;

public class MovieDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public string PosterPath { get; set; } = string.Empty;
    public string BackdropPath { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
    public double VoteAverage { get; set; }

    public List<string> ImageGallery { get; set; } = [];
    public List<CastDto> Cast { get; set; } = [];
    public List<GenreDto> Genres { get; set; } = [];
    public List<CommentDto> Comments { get; set; } = [];
}