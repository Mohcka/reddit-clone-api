using System.Threading.Tasks;
using reddit_clone_api.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace reddit_clone_api.Services
{
  public interface IPostService {
    /// <summary>
    /// Get a list of posts
    /// </summary>
    /// <returns></returns>
    List<Post> Get();
    Post Get(string id);

    Task<IEnumerable<Post>> GetPostsByUserId(string id);
    Post Create(Post post);
    void Update(string id, Post postin);
    void Remove(string id);

  }

  public class PostService : IPostService
  {
    private readonly IMongoCollection<Post> _posts;

    public PostService(IRedditCloneDatabaseSettings settings)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _posts = database.GetCollection<Post>(settings.PostsCollectionName);
    }

    public List<Post> Get() =>
        _posts.Find(post => true).ToList();

    public Post Get(string id) =>
        _posts.Find<Post>(post => post.Id == id).FirstOrDefault();

    public async Task<IEnumerable<Post>> GetPostsByUserId(string id)
    {
      return await _posts.Find<Post>(p => p.UserId == id).ToListAsync();
    }

    public Post Create(Post post)
    {
      _posts.InsertOne(post);
      return post;
    }

    public void Update(string id, Post postIn) =>
        _posts.ReplaceOne(post => post.Id == id, postIn);

    public void Remove(Post postIn) =>
        _posts.DeleteOne(post => post.Id == postIn.Id);

    public void Remove(string id) =>
        _posts.DeleteOne(post => post.Id == id);
  }
}