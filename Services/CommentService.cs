using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

using reddit_clone_api.Models;

namespace reddit_clone_api.Services
{
  public interface ICommentService : IBaseServiceMDB<Comment>
  {
    Task<List<Comment>> GetCommentsByPostId(string Id);
    Task<Comment> VoteOnComment(Comment comment, VoteType vote, int offset = 1);
  }

  public class CommentService : BaseServiceMDB<Comment>, ICommentService
  {
    public CommentService(IRedditCloneDatabaseSettings settings) : base(settings, settings.CommentsCollectionName)
    {

    }

    public async Task<List<Comment>> GetCommentsByPostId(string postId) {
      return await _db.Find<Comment>(c => c.PostId == postId).ToListAsync();
    }

    /// <summary>
    /// Updates the comment numVotes by the vote type and the specified offset
    /// </summary>
    /// <param name="comment">The comment being voted on</param>
    /// <param name="vote">Enum to determin if the vote is a voteup or a votedown</param>
    /// <param name="offset">The amount to alter the number of votes by</param>
    /// <returns></returns>
    public async Task<Comment> VoteOnComment(Comment comment, VoteType vote, int offset = 1) {
      if(vote == VoteType.Up){
        comment.NumVotes += offset;
      } else {
        comment.NumVotes -= offset;
      }

      await Update(comment);

      return comment;
    }
  }
}