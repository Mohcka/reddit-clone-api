using System.ComponentModel.DataAnnotations;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace reddit_clone_api.Models
{
  /// <summary>
  /// TODO: Write docs
  /// </summary>
  public class Post
  {
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Title")]
    [Required]
    public string PostTitle { get; set; }
    [BsonElement("Post")]
    [Required]
    public string PostContent { get; set; }

    [Required]
    public string UserId { get; set; }

  }
}