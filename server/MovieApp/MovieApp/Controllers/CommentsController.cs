using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Domain.Commands.Comments;
using MovieApp.Services.Comments;

namespace MovieApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentsCommandService _commentService;

    public CommentsController(ICommentsCommandService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateMovieCommentCommand command, CancellationToken token)
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value!);

        await _commentService.AddCommentAsync(command, userId, token);
        return NoContent();
    }
}