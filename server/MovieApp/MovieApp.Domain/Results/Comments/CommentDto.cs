using MovieApp.Domain.Results.Users;

namespace MovieApp.Domain.Results.Comments;

public class CommentDto
{
    public Guid Id { get; set; }
    public UserDto User { get; set; } = default!;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}