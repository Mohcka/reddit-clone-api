using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace reddit_clone_api.Models {
  public interface IMDBDocument {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    string Id { get; set; }
  }
}