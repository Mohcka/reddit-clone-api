using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using reddit_clone_api.Models;
using reddit_clone_api.Models.DTO;
using reddit_clone_api.Services;

namespace redit_clone_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [EnableCors("public")]
  public class PostsController : ControllerBase
  {
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private readonly IVoteService _voteService;
    private readonly IUserService _userService;


    public PostsController(IPostService postService, ICommentService commentService, IVoteService voteService, IUserService userService)
    {
      _postService = postService;
      _commentService = commentService;
      _voteService = voteService;
      _userService = userService;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<Post>> Get() => _postService.Get();

    [HttpGet("{id:length(24)}")]
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
      var userId = User.FindFirst(ClaimTypes.Name)?.Value; // User who created this post

      _postService.Create(new Post{
        PostTitle = post.PostTitle,
        PostContent = post.PostContent,
        UserId = userId
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

    [HttpGet("/{id:length(24)}/comment")]
    public async Task<IActionResult> GetWithComments(string id){
      List<Comment> comments;

      var post = _postService.Get(id);

      if (post == null)
      {
        return NotFound(new { message = "Post wasn't found" });
      }
      
      comments = await _commentService.GetCommentsByPostId(id);

      return Ok(comments);
    }

    [HttpPost("{id:length(24)}/vote")]
    [Authorize]
    public async Task<IActionResult> VoteOnPost(string id, [Required]string voteType) {
      var userId = User.FindFirst(ClaimTypes.Name)?.Value;
      // Check if the request sent a valid vote
      if(!Enum.IsDefined(typeof(VoteType), voteType)) {
        return BadRequest( new { message = "Invalid vote"});
      }
      // Create var with enum value
      Enum.TryParse(voteType, out VoteType vote);

      var post = _postService.Get(id);

      if(post == null) {
        return NotFound( new { message = "Post was not found"});
      }
      // Update vote num for post
      _postService.VoteOnPost(post, vote);
      // Add vote record
      await  _voteService.Add(new Vote {
        UserVote = vote,
        UserId = userId,
        ResponseId = post.Id
      });

      return Ok();
    }
  }
}