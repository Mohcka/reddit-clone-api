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
    public ActionResult<Post> Get(string id)
    {
      //TODO: validate user exists
      if (id.Length != 24) return BadRequest(new { message = "Invalid id" });

      var user = _postService.Get(id);



      if (user == null) return BadRequest(new { message = "User does not exist" });

      return Ok(user);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreatePostDTO post)
    {
      var userId = User.FindFirst(ClaimTypes.Name)?.Value; // User who created this post

      var user = await _userService.GetUser(userId);


      _postService.Create(new Post
      {
        PostTitle = post.PostTitle,
        PostContent = post.PostContent,
        UserId = userId,
        UserName = user.UserName
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
    public IActionResult Update([FromBody] Post postIn)
    {
      var post = _postService.Get(postIn.Id);

      if (post == null)
      {
        return NotFound(new { message = "Post to update was not found" });
      }

      _postService.Update(post.Id, postIn);

      return Ok();
    }

    [HttpGet("{id:length(24)}/comments")]
    public async Task<IActionResult> GetWithComments(string id)
    {
      List<Comment> comments;

      var post = _postService.Get(id);

      if (post == null)
      {
        return NotFound(new { message = "Post wasn't found" });
      }

      comments = await _commentService.GetCommentsByPostId(id);

      return Ok(new PostWithCommentsResponseDTO
      {
        Post = post,
        Comments = comments
      });
    }

    [HttpPost("{id:length(24)}/vote")]
    [Authorize]
    public async Task<IActionResult> VoteOnPost(string id, [Required] string voteType)
    {
      var userId = User.FindFirst(ClaimTypes.Name)?.Value;
      // Check if the request sent a valid vote
      if (!Enum.IsDefined(typeof(VoteType), voteType))
      {
        return BadRequest(new { message = "Invalid vote" });
      }
      // Create var with enum value
      Enum.TryParse(voteType, out VoteType vote);

      var post = _postService.Get(id);

      if (post == null)
      {
        return NotFound(new { message = "Post was not found" });
      }

      // Used to determine if a vote should be cancelled or reversed if it exists
      var foundVote = await _voteService.FindByUserAndResponseId(userId, post.Id);

      if (foundVote == null)
      {
        // No vote has been registered, so create one and add to tally
        post = _postService.VoteOnPost(post, vote);
        await _voteService.Add(new Vote
        {
          UserVote = vote,
          UserId = userId,
          ResponseId = post.Id
        });
      }
      else if (foundVote.UserVote == vote)
      {
        // Clear vote if same vote was selected

        // Set to opposite vote to negate user's vote
        var negatingVote = vote == VoteType.Up ? VoteType.Down : VoteType.Up;

        post = _postService.VoteOnPost(post, negatingVote);
        await _voteService.Remove(foundVote.Id);
      }
      else
      {
        // Can be implied at this point that the user changed their vote
        // to the opposite

        // Since they chose the opposite vote, offset is set to two
        post = _postService.VoteOnPost(post, vote, 2);

        // Set vote document to the opposite vote
        foundVote.UserVote = vote;
        await _voteService.Update(foundVote);
      }

      return Ok(new VoteResponseDTO
      {
        UserVote = Enum.GetName(typeof(VoteType), vote),
        NumVotes = post.NumVotes
      });
    }
  }
}