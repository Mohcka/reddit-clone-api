using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

using reddit_clone_api.Models;

namespace reddit_clone_api.Services
{
  /// <summary>
  /// Collection of methods for retrieving and handling data from a MongoDB database
  /// </summary>
  /// <typeparam name="T">The type that represents the schema of a collection</typeparam>
  public interface IBaseServiceMDB<T> where T : IMDBDocument
  {
    /// <summary>
    /// Store a collection into a list
    /// </summary>
    /// <returns>A list of all documents of a given type</returns>
    Task<List<T>> GetAll();
    /// <summary>
    /// Return a document using a unique id
    /// </summary>
    /// <param name="id">Unique id of the specified document</param>
    /// <returns>The found document or null if none was found</returns>
    Task<T> Get(string id);
    /// <summary>
    /// Inserts the document's properties into the DB
    /// </summary>
    /// <param name="document">The document to insert</param>
    /// <returns>The inserted document</returns>
    Task<T> Add(T document);
    /// <summary>
    /// Updates an existing document found within the DB
    /// </summary>
    /// <param name="document">An document with updated properties</param>
    /// <returns>The updated document</returns>
    Task<T> Update(T document);
    /// <summary>
    /// Finds an document by id then deletes it
    /// </summary>
    /// <param name="id">ID of the document to find</param>
    /// <returns>The deleted document</returns>
    Task Remove(string id);
    Task Remove(T document);
  }

  public abstract class BaseServiceMDB<T> : IBaseServiceMDB<T>
  where T : IMDBDocument
  {

    protected readonly IMongoCollection<T> _db;

    public BaseServiceMDB(IRedditCloneDatabaseSettings settings, string collection)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _db = database.GetCollection<T>(collection);
    }

    public async Task<List<T>> GetAll()
    {
      return await _db.Find<T>(document => true).ToListAsync();
    }

    public async Task<T> Get(string id)
    {
      return await _db.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<T> Add(T document)
    {
      await _db.InsertOneAsync(document);

      return document;
    }

    public async Task<T> Update(T documentIn)
    {
      await _db.ReplaceOneAsync(d => d.Id == documentIn.Id, documentIn);

      return documentIn;
    }

    public async Task Remove(string id)
    {
      await _db.DeleteOneAsync(d => d.Id == id);
    }

    public async Task Remove(T document) {
      await _db.DeleteOneAsync(d => d.Id == document.Id);
    }
  }
}