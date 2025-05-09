namespace MovieApp.Domain.Commands.Comments;

public class CreateMovieCommentCommand
{
    public int MovieId { get; set; }

    public string Content { get; set; } = null!;
}