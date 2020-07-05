using System.Security.Claims;
using reddit_clone_api.Models;
using reddit_clone_api.Models.DTO;
using reddit_clone_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;

namespace redit_clone_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [EnableCors("public")]
  public class PostsController : ControllerBase
  {
    private readonly IPostService _postService;


    public PostsController(IPostService postService)
    {
      _postService = postService;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<Post>> Get() => _postService.Get();

    [HttpGet("{id:length(24)}")]
    [Authorize]
    public ActionResult<Post> Get(string id){
      //TODO: validate user exists
      if(id.Length != 24) return BadRequest(new { message = "Invalid id"});

      var user =_postService.Get(id);

      

      if(user == null) return BadRequest(new {message = "User does not exist"});

      return Ok(user);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody]CreatePostDTO post)
    {
      // TODO: add some validation
      ModelState.Remove("Id"); // Id will be created for post
      ModelState.Remove("UserId"); // Id will be created for post
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // User who created this post

      _postService.Create(new Post{
        PostTitle = post.PostTitle,
        PostContent = post.PostContent,
        UserId = post.UserId
      });

      return Ok();
    }

    /// <summary>
    /// Requests an existing post to be updated, responds ok on success
    /// </summary>
    /// <param name="postIn">Data of the post to update</param>
    /// <returns>Action Result with status of update</returns>
    [HttpPut]
    [Authorize]
    public IActionResult Update([FromBody] Post postIn) {
      var post = _postService.Get(postIn.Id);

      if(post == null) {
        return NotFound(new { message = "Post to update was not found"});
      } 

      _postService.Update(post.Id, postIn);

      return Ok();
    }
  }
}