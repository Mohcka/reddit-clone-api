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

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
      var comment = await _commentService.Get(id);

      if (comment == null)
      {
        return NotFound(new { message = "Comment was not found" });
      }

      return Ok(comment);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCommentRequestDTO comment)
    {
      var userId = User.FindFirst(ClaimTypes.Name)?.Value;

      var user = await _userService.GetUser(userId);

      await _commentService.Add(new Comment
      {
        UserComment = comment.UserComment,
        PostId = comment.PostId,
        UserId = userId,
        Username = user.UserName
      });

      return Ok();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] Comment commentIn)
    {
      var comment = _commentService.Get(commentIn.Id);

      if (comment == null)
      {
        return NotFound(new { message = "Comment was not found" });
      }

      await _commentService.Update(commentIn);

      return Ok();
    }

    [HttpPost("{id:length(24)}/vote")]
    [Authorize]
    public async Task<IActionResult> VoteOnComment(string id, [Required] string voteType)
    {
      var userId = User.FindFirst(ClaimTypes.Name)?.Value;
      // Check if the request sent a valid vote
      if (!Enum.IsDefined(typeof(VoteType), voteType))
      {
        return BadRequest(new { message = "Invalid vote" });
      }
      // Create var with enum value

      Enum.TryParse(voteType, out VoteType vote);
      var comment = await _commentService.Get(id);

      if (comment == null)
      {
        return NotFound(new { message = "Comment was not found" });
      }

      // Used to determine if a vote should be cancelled or reversed if it exists
      var foundVote = await _voteService.FindByUserAndResponseId(userId, comment.Id);

      if (foundVote == null)
      {
        // No vote has been registered, so create one and add to tally
        comment = await _commentService.VoteOnComment(comment, vote);
        await _voteService.Add(new Vote
        {
          UserVote = vote,
          UserId = userId,
          ResponseId = comment.Id
        });
      }
      else if (foundVote.UserVote == vote)
      {
        // Clear vote if same vote was selected

        // Set to opposite vote to negate user's vote
        var negatingVote = vote == VoteType.Up ? VoteType.Down : VoteType.Up;

        comment = await _commentService.VoteOnComment(comment, negatingVote);
        await _voteService.Remove(foundVote.Id);
      }
      else
      {
        // Can be implied at this point that the user changed their vote
        // to the opposite

        // Since they chose the opposite vote, offset is set to two
        comment = await _commentService.VoteOnComment(comment, vote, 2);

        // Set vote document to the opposite vote
        foundVote.UserVote = vote;
        await _voteService.Update(foundVote);
      }

      return Ok(new VoteResponseDTO
      {
        UserVote = Enum.GetName(typeof(VoteType), vote),
        NumVotes = comment.NumVotes
      });
    }
  }
}