using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{

}