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

    // init seed data
    // private void seedData()
    // {
    //   if (_posts.Find(post => true).CountDocuments() == 0)
    //   {
    //     Post firstPost = new Post
    //     {
    //       PostTitle = "Title",
    //       PostContent = "Some content"
    //     };
    //     Post secondPost = new Post
    //     {
    //       PostTitle = "Another Title",
    //       PostContent = "More content"
    //     };

    //     _posts.InsertMany(new List<Post> { firstPost, secondPost });
    //   }
    // }
  }

  public interface IRedditCloneDatabaseSettings
  {
    string PostsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
  }
}