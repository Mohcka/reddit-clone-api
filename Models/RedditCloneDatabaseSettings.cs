using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;

namespace reddit_clone_api.Models
{
  public class RedditCloneDatabaseSettings : IRedditCloneDatabaseSettings
  {
    public string PostsCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }

  public interface IRedditCloneDatabaseSettings
  {
    string PostsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
  }
}