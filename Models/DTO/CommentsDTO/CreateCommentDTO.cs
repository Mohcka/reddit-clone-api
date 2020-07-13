using System.ComponentModel.DataAnnotations;

using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace reddit_clone_api.Models.DTO
{
  public class CreateCommentRequestDTO
  {
    [Required]
    public string UserComment { get; set; }
    [Required]
    public string PostId { get; set; }
  }
}