using MovieApp.Domain.Commands.Comments;
using MovieApp.Domain.Models;
using MovieApp.Infrastructure.Persistence;

namespace MovieApp.Services.Comments;

public class CommentsCommandService : ICommentsCommandService
{
    private readonly MovieAppDbContext _context;

    public CommentsCommandService(MovieAppDbContext context)
    {
        _context = context;
    }

    public async Task AddCommentAsync(CreateMovieCommentCommand command, Guid userId, CancellationToken token = default)
    {
        var comment = new Comment
        {
            MovieId = command.MovieId,
            Content = command.Content,
            UserId = userId,
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(token);
    }
}