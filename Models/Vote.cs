using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace reddit_clone_api.Models {
  public enum VoteType {
    Up,
    Down
  }
  public class Vote : IMDBDocument {
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("UserVote")]
    [Required]
    public VoteType UserVote { get; set; }
    [BsonElement("UserId")]
    [Required]
    public string UserId { get; set; }
    /// <summary>
    /// The id of either the post or comment
    /// </summary>
    /// <value></value>
    [BsonElement("ResponseId")]
    [Required]
    public string ResponseId { get; set; }
  }
}