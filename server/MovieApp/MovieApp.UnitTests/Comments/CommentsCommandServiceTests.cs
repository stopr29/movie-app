using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Commands.Comments;
using MovieApp.Services.Comments;

namespace MovieApp.UnitTests.Comments;

public class CommentsCommandServiceTests
{
    [Fact]
    public async Task AddCommentAsync_ShouldAddCommentToDb()
    {
        var dbContext = TestDbContextFactory.CreateInMemoryDbContext();
        var service = new CommentsCommandService(dbContext);

        var command = new CreateMovieCommentCommand
        {
            MovieId = 12345,
            Content = "Awesome movie!"
        };

        var userId = Guid.NewGuid();

        await service.AddCommentAsync(command, userId);

        var comment = await dbContext.Comments.FirstOrDefaultAsync();
        Assert.NotNull(comment);
        Assert.Equal(command.MovieId, comment.MovieId);
        Assert.Equal(command.Content, comment.Content);
        Assert.Equal(userId, comment.UserId);
    }
}