using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace reddit_clone_api.Models
{
  /// <summary>
  /// TODO: Write docs
  /// </summary>
  public class User : IMDBDocument
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string Id { get; set; }
    [BsonElement("username")]
    [Required]
    public string UserName { get; set; }

    [JsonIgnore] // For security, ignore this when serialzing
    [BsonElement("password")]
    [Required]
    public string Password { get; set; }
  }
}