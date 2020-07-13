using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using reddit_clone_api.Models;
using reddit_clone_api.Models.DTO;
using reddit_clone_api.Services;

namespace reddit_clone_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [EnableCors("public")]
  public class CommentsController : ControllerBase
  {
    private readonly ICommentService _commentService;
    private readonly IPostService _postService;
    private readonly IVoteService _voteService;
    private readonly IUserService _userService;

    public CommentsController(ICommentService commentService, IPostService postService, IVoteService voteService, IUserService userService)
    {
      _commentService = commentService;
      _postService = postService;
      _voteService = voteService;
      _userService = userService;
    }

    /// <summary>
    /// Fetches all comments for a post based on it's Id
    /// </summary>
    /// <param name="postId">Id of the post</param>
    /// <returns></returns>
    [HttpGet("/{postId:length(24)}")]
    public async Task<IActionResult> getAllPostComments(string postId)
    {
      List<Comment> comments;

      var post = _postService.Get(postId);

      if (post == null)
      {
        return NotFound(new { message = "Post wasn't found" });
      }
      
      comments = await _commentService.GetCommentsByPostId(postId);

      return Ok(comments);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody]CreateCommentDTO comment) {
      var userId = User.FindFirst(ClaimTypes.Name)?.Value;

      await _commentService.Add(new Comment{
        UserComment = comment.UserComment,
        UserId = userId,
        PostId = comment.PostId
      });

      return Ok();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] Comment commentIn) {
      var comment = _commentService.Get(commentIn.Id);

      if(comment == null){
        return NotFound(new { message = "Comment was not found"});
      }

      await _commentService.Update(commentIn);

      return Ok();
    }

    [HttpPost("{id:length(24)}/vote")]
    [Authorize]
    public async Task<IActionResult> VoteOnComment(string id, [Required] string voteType)
      var userId = User.FindFirst(ClaimTypes.Name)?.Value;
      // Check if the request sent a valid vote
      if(!Enum.IsDefined(typeof(VoteType), voteType)) {
        return BadRequest( new { message = "Invalid vote"});
      }
      // Create var with enum value

      Enum.TryParse(voteType, out VoteType vote);
      var comment = await _commentService.Get(id);

      if(comment == null) {
        return NotFound(new { message = "Post was not found" });
      }

      await _commentService.VoteOnComment(comment, vote);

      return Ok();
    }
  }
}