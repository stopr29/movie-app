using MovieApp.Domain.Commands.Comments;

namespace MovieApp.Services.Comments;

public interface ICommentsCommandService
{
    Task AddCommentAsync(CreateMovieCommentCommand command, Guid userId, CancellationToken token = default);
}