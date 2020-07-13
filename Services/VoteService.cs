using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

using reddit_clone_api.Models;

namespace reddit_clone_api.Services
{
  public interface IVoteService : IBaseServiceMDB<Vote>
  {
    Task<List<Vote>> GetUserVotes(string userId);
    Task<Vote> FindByUserAndResponseId(string userId, string responseId);
    
  }

  public class VoteService : BaseServiceMDB<Vote>, IVoteService
  {
    public VoteService(IRedditCloneDatabaseSettings settings) : base(settings, settings.VotesCollectionName)
    {

    }

    public async Task<List<Vote>> GetUserVotes(string userId) {
      return await _db.Find<Vote>(v => v.UserId == userId).ToListAsync();
    }

    public async Task<Vote> FindByUserId(string userId) {
      return await _db.Find<Vote>(v => v.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<Vote> FindByUserAndResponseId(string userId, string responseId) {
      return await _db.Find<Vote>(v => v.UserId == userId && v.ResponseId == responseId).FirstOrDefaultAsync();
    }
  }
}