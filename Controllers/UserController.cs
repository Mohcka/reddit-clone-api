using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using reddit_clone_api.Services;
using reddit_clone_api.Models;
using reddit_clone_api.Models.DTO;

namespace reddit_clone_api.Controllers
{

  /// <summary>
  /// TODO: Write docs
  /// </summary>
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  [EnableCors("Public")]
  public class UsersController : ControllerBase
  {
    private IUserService _userService;
    private IPostService _postService;

    public UsersController(IUserService userService, IPostService postService)
    {
      _userService = userService;
      _postService = postService;
    }

    /// <summary>
    /// TODO: Write docs
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Authenticate([FromBody] AuthenticateRequestDTO model)
    {
      // Request for token based on submitted credentials
      var resp = _userService.Authenticate(model);

      // No user found from service
      if (resp == null) return BadRequest(new { message = "Username or password was incorrect" });

      // Checksout, return token to client
      return Ok(resp);
    }

    [HttpGet("{id:length(24)}/posts")]
    public async Task<IActionResult> GetUserWithPosts(string id)
    {
      var user = await _userService.GetUser(id);

      if (user == null)
      {
        return NotFound(new { error = new { message = "User was not found" } });
      }

      var posts = await _postService.GetPostsByUserId(user.Id);

      return Ok(new UserWithPostsResponseDTO
      {
        Username = user.UserName,
        Posts = posts
      });
    }

    /// <summary>
    /// TODO: Write docs
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAll()
    {
      var users = _userService.GetAll();
      return Ok(users);
    }
  }
}