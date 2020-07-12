using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

using reddit_clone_api.Models;

namespace reddit_clone_api.Services
{
  public interface ICommentService : IBaseServiceMDB<Comment>
  {
    Task<List<Comment>> GetCommentsByPostId(string Id);
    Task<Comment> VoteOnComment(Comment comment1, VoteType vote);
  }

  public class CommentService : BaseServiceMDB<Comment>, ICommentService
  {
    public CommentService(IRedditCloneDatabaseSettings settings) : base(settings, settings.CommentsCollectionName)
    {

    }

    public async Task<List<Comment>> GetCommentsByPostId(string postId) {
      return await _db.Find<Comment>(c => c.Id == postId).ToListAsync();
    }

    public async Task<Comment> VoteOnComment(Comment comment, VoteType vote) {
      if(vote == VoteType.Up){
        comment.NumVotes++;
      } else {
        comment.NumVotes--;
      }

      await Update(comment);

      return comment;
    }
  }
}