using System.ComponentModel.DataAnnotations;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace reddit_clone_api.Models {
  public class Comment : IMDBDocument {
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [Required]
    public string UserComment { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public string PostId { get; set; }
    [Required]
    public int NumVotes { get; set; } = 0;
  }
}