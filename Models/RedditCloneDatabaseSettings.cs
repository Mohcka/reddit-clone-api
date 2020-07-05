using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;

namespace reddit_clone_api.Models
{
  public class RedditCloneDatabaseSettings : IRedditCloneDatabaseSettings
  {
    public string secret { get; set; }
    public string UsersCollectionName { get; set; }
    public string PostsCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }

  public interface IRedditCloneDatabaseSettings
  {
    string secret { get; set; }
    string UsersCollectionName { get; set; }
    string PostsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
  }
}